namespace :nuget do
  buildsupport_path = File.dirname(__FILE__)
  nuget = "#{buildsupport_path}/nuget.exe"
  nugroot = File.expand_path(ENV['NUGET_HUB'] || "/nugs")
  
  
  desc "Build the nuget package"
  task :build do
    rm Dir.glob("#{ARTIFACTS}/*.nupkg")
    FileList["packaging/nuget/*.nuspec"].each do |spec|
      sh "#{nuget} pack #{spec} -o #{ARTIFACTS} -Version #{BUILD_NUMBER} -Symbols"
    end
  end
  
  desc "update dependencies from local machine"
  task :pull, [:package] do |task, args|
    Nuget.each_installed_package do |package|
      next if args[:package] && Nuget.package_name(package).downcase != args[:package].downcase
      src_package = File.join nugroot, Nuget.package_name(package)
      if File.directory? src_package
        ['lib','tools'].each do |folder|
          dst = File.join package, folder 
          src = File.join src_package, folder 
          if File.directory? src
            clean_dir dst
            cp_r src + "/.", dst, :verbose => false
            after_nuget_update(Nuget.package_name(package), dst) if respond_to? :after_nuget_update
          end
        end
        puts "pulled from #{src_package}"
      else
        puts "could not find #{src_package}"
      end
    end
  end

  desc "Updates dependencies from nuget.org"
  task :update => [:update_packages, :clean, :update_callback]

  task :update_packages do
    FileList["**/*.sln"].each do |proj|
      sh "#{nuget} update #{proj}"
    end
    FileList["**/packages.config"].each do |proj|
      sh "#{nuget} install #{proj} -OutputDirectory #{Nuget.package_root}"
    end
  end
  
  task :update_callback do
    after_nuget_update(nil, Nuget.package_root) if respond_to? :after_nuget_update
  end

  desc "pushes dependencies to central location on local machine for nuget:pull from other repos"
  task :push, [:package] => :build do |task, args|
    FileList["#{ARTIFACTS}/*.nupkg"].exclude(".symbols.nupkg").each do |file|
      next if args[:package] && Nuget.package_name(file).downcase != args[:package].downcase
      destination = File.join nugroot, Nuget.package_name(file)
      clean_dir destination
      unzip_file file, destination
      puts "pushed to #{destination}"
    end
  end

  def clean_dir(path)
    mkdir_p path, :verbose => false
    rm_r Dir.glob(File.join(path, "*.*")), :verbose => false
  end
		
  def unzip_file (file, destination)
    require 'rubygems'
    require 'zip/zip'
    Zip::ZipFile.open(file) { |zip_file|
     zip_file.each { |f|
       f_path=File.join(destination, f.name)
       FileUtils.mkdir_p(File.dirname(f_path))
       zip_file.extract(f, f_path) unless File.exist?(f_path)
     }
    }
  end

  desc "Pushes nuget packages to the official feed"
  task :release, [:package] do |t, args|
    require 'open-uri'
    release_path = "#{buildsupport_path}/nuget_release"
    clean_dir release_path

    artifact_url = "http://teamcity.codebetter.com/guestAuth/repository/downloadAll/#{@teamcity_build_id}/.lastSuccessful/artifacts.zip"
    puts "downloading artifacts from #{artifact_url}"
    artifact = open(artifact_url)
    unzip_file artifact.path, release_path
    FileList["#{release_path}/*.nupkg"].exclude(".symbols.nupkg").each do |nupkg|
      next if args[:package] && Nuget.package_name(nupkg).downcase != args[:package].downcase
      sh "#{nuget} push #{nupkg}" do |ok, res|
        puts "May not have published #{nupkg}" unless ok
      end
    end
  end	

  task :clean do
    require 'rexml/document'
    repo_paths = Nuget.repositories
    packages = repo_paths.map{|repo| Nuget.packages(repo)}.reduce(:|)

    Nuget.each_installed_package do |package|
      name = Nuget.package_name(package)
      tracked = packages.select{|p| p.include? name} 
      if tracked.any? # only remove folders for older versions of tracked packages
        should_delete = !tracked.detect{|t| package.end_with? t[name]}
        rm_rf package if should_delete
      end
    end

  end
end

namespace :ripple do
  @ripple_root = File.expand_path(File.join(File.dirname(__FILE__), "../../") + "/ripple")
  @progress_file = "#{@ripple_root}/packages/ripple.txt"

  task :local => ["nuget:pull", :ci, "nuget:push"]
  task :public => [:gitupdate, "nuget:update", :ci, :commit, :manualsteps]

  task :gitupdate do
    #raise "Uncommitted changes in #{Dir.pwd}" unless `git status --porcelain`.strip.empty?
    sh "git checkout master && git pull --ff-only" do |ok, status|
      raise "Cannot pull latest into #{Dir.pwd}. You need to rebase or merge origin/master" unless ok
    end
  end

  task :commit do
    status = `git status --porcelain`
    changes = status.split("\n").select{|l| l.start_with? "??"}.map{|l| l.match(/packages\/(.*)\//)[1]}
    if changes.any?
      msg = "Updated packages: #{changes.join(", ")}"
      sh "git add -A"
      sh "git commit -m \"#{msg}\""
    else
      puts "no changes to commit in #{Dir.pwd}"
    end
  end

  task :manualsteps do
    puts """
*******************************
1) Push your changes to github. 
2) Optionally kick off the teamcity build manually (if it hasn't started automatically yet)
3) Wait for teamcity build to finish
4) Run rake ripple:publish  to continue after publishing nuget packages publicly
   or  rake ripple:continue to continue without publishing publicly
"""
    sh "start http://teamcity.codebetter.com/viewType.html?buildTypeId=#{@teamcity_build_id}"
  end

  task :continue do
    job = get_next_job 
    if job
      puts "mark #{job} as complete"
      mark_complete job

      run_in @ripple_root, "rake ripple:nextjob"
    else
      puts "no current job to continue"
    end
  end

  task :publish => ["nuget:release", :continue]

  def mark_complete(job)
    jobs = get_ripple_jobs
    raise "Job does not exist: #{job} -- cannot mark it complete" unless jobs.include? job
    init_ripple_jobs(jobs - [job])
  end

  def get_next_job
    jobs = get_ripple_jobs
    return nil unless jobs.any?
    jobs[0]
  end

  def get_ripple_jobs
    File.readlines(@progress_file).map{|x| x.chomp}.reject{|x| x.empty?}
  end

  def init_ripple_jobs(jobs)
    File.open(@progress_file, "w") do |file|
      jobs.each{|job| file.puts job}
    end
  end
end


module Nuget
  def self.each_installed_package
    FileList[File.join(package_root, "*")].exclude{|f| File.file?(f)}.each do |package|
      yield package
    end
  end

  def self.packages(package_config)
    xml = REXML::Document.new File.read package_config
    xml.get_elements('packages/package').map do |p|
      { p.attributes['id'] => p.attributes['version'] } 
    end
  end

  def self.repositories

    #get packages.config from nuget's repositories.config file in the package root
    repo_file_path = File.join package_root, "repositories.config"
    if FileTest.exists? repo_file_path 
      file = REXML::Document.new File.read repo_file_path
      return file.get_elements("repositories/repository").map{|node| File.expand_path(File.join(package_root, node.attributes["path"]))}
    end 
    
    #otherwise glob all the packages.config in the source directory
    Dir.glob("{source,src}/**/packages.config")
  end

  def self.package_root
    root = nil
    
    packroots = Dir.glob("{source,src}/packages")

    return packroots.last if packroots.length > 0

    Dir.glob("{source,src}").each do |d|
      packroot = File.join d, "packages"
      FileUtils.mkdir_p(packroot) 
      root = packroot
    end       

    root
  end

  def self.package_name(filename)
    File.basename(filename, ".nupkg").gsub(/[\d.]+$/, "")
  end

  def self.tool(package, tool)
    File.join(Dir.glob(File.join(package_root,"#{package}.*")).sort.last, "tools", tool)
  end
end

def run_in(working_dir, cmd)
  sh "buildsupport/run_in_path.cmd #{working_dir.gsub('/','\\')} #{cmd}"
end
