
	desc "Restores nuget package files"
	task :restore do
	  puts 'Restoring all the nuget package files'
	  restore
	end
	
	desc "Updates nuget package files to the latest"
	task :update do
	  puts 'Updating all the nuget package files'
	  update
	end

	desc "For CI mode, replaces all dependencies with the latest, greatest version of all"
	task :update_all_dependencies do
	  ripple 'clean'
	  update
	  restore
	end
	
	desc "restore packages if the files don't seem to exist"
	task :restore_if_missing do
	  packageFiles = Dir["#{File.dirname(__FILE__)}/src/packages/*.dll"]
	  restore unless packageFiles.any?
	end
	
	desc "creates a history file for nuget dependencies"
	task :history do
	  ripple 'history'
	end
	
	desc "publishes all the nuget's published by this solution"
	task :publish do
	  nuget_api_key = ENV['apikey']
	  server = ENV['server']
	  cmd = "publish #{BUILD_NUMBER} #{nuget_api_key}"
	  cmd = cmd + " --server #{server}" unless server.nil?
	  ripple cmd
	end
	
	desc "packages the nuget files from the nuspec files in packaging/nuget and publishes to /artifacts"
	task :package => [:history] do
		cmd = "local-nuget --version #{BUILD_NUMBER} --destination artifacts"
		ripple cmd
	end

	def self.update()
	  cmd = "update"
	  ripple cmd
	  # ripple try_add_feeds cmd	
	end
	
	def self.restore()
	  cmd = "restore"
	  ripple cmd
	  # ripple try_add_feeds cmd	
	end

	def self.try_add_feeds(cmd)
	  feeds = ENV['feeds']
	  feeds = 'http://build.fubu-project.org/guestAuth/app/nuget/v1/FeedService.svc/#http://packages.nuget.org/v1/FeedService.svc/#http://nuget.org/api/v2' if feeds.nil?
	  
	  cmd = cmd + " --feeds #{feeds}" unless feeds.nil?
	  cmd
	end
	
	def self.ripple(args)
	  ripple = Platform.runtime("buildsupport/ripple.exe") 
	  sh "#{ripple} #{args}"
	end