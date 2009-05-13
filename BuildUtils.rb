require 'rubygems'

require 'erb'
require 'activesupport'
require 'find'
require 'zip/zip'
require 'fileutils'

class NUnitRunner
	include FileTest

	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, 'x86')
		@compileTarget = paths.fetch(:compilemode, 'debug')
		
		if ENV["teamcity.dotnet.nunitlauncher"] # check if we are running in TeamCity
			# We are not using the TeamCity nunit launcher. We use NUnit with the TeamCity NUnit Addin which needs tO be copied to our NUnit addins folder
			# http://blogs.jetbrains.com/teamcity/2008/07/28/unfolding-teamcity-addin-for-nunit-secrets/
			# The teamcity.dotnet.nunitaddin environment variable is not available until TeamCity 4.0, so we hardcode it for now
			@teamCityAddinPath = ENV["teamcity.dotnet.nunitaddin"] ? ENV["teamcity.dotnet.nunitaddin"] : 'c:/TeamCity/buildAgent/plugins/dotnetPlugin/bin/JetBrains.TeamCity.NUnitAddin-NUnit'
			cp @teamCityAddinPath + '-2.4.7.dll', 'tools/nunit/addins'
		end
	
		@nunitExe = File.join('tools', 'nunit', "nunit-console#{(@compilePlatform.nil? ? '' : "-#{@compilePlatform}")}.exe").gsub('/','\\') + ' /nothread'
	end
	
	def executeTests(assemblies)
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{(@compilePlatform.nil? ? '' : "#{@compilePlatform}/")}#{@compileTarget}/#{assem}.dll")
			sh "#{@nunitExe} \"#{file}\""
		end
	end
end

class MSBuildRunner
	def self.compile(attributes)
		version = attributes.fetch(:clrversion, 'v3.5')
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    solutionFile = attributes[:solutionfile]
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', version)
		msbuildFile = File.join(frameworkDir, 'msbuild.exe')
		
		sh "#{msbuildFile} #{solutionFile} /maxcpucount /v:m /property:BuildInParallel=false /property:Configuration=#{compileTarget} /t:Rebuild"
	end
end

class AspNetCompilerRunner
	def self.compile(attributes)
		
		webPhysDir = attributes.fetch(:webPhysDir, '')
		webVirDir = attributes.fetch(:webVirDir, '')
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', 'v2.0.50727')
		aspNetCompiler = File.join(frameworkDir, 'aspnet_compiler.exe')

		sh "#{aspNetCompiler} -p #{webPhysDir} -v #{webVirDir}"
	end
end

class AsmInfoBuilder
	attr_reader :buildnumber, :parameterless_attributes

	def initialize(version, properties)
		@properties = properties
		@buildnumber = version
		@properties['Version'] = @properties['InformationalVersion'] = buildnumber;
		@parameterless_attributes = [:allow_partially_trusted_callers]
	end
	
	def write(file)
		template = %q{using System.Reflection;
using System.Security;

<% @properties.each do |k, v| %>
<% if @parameterless_attributes.include? k %>
[assembly: <%= k.to_s.camelize %>]
<% else %>
[assembly: Assembly<%= k.to_s.camelize %>("<%= v %>")]
<% end %>
<% end %>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open(file, 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end

class InstallUtilRunner
	def installServices(services, parameters)
		services.each do |service|
			params = ""
			parameters.each_pair {|key, value| params = params + "/" + key + "=" + value + " "}
			sh "tools/installutil /i #{params} #{service}"
		end
	end
	
	def uninstallServices(services)
		services.each do |service|
			begin
				sh "tools/installutil /u #{service}"
			rescue Exception => e
				puts 'IGNORING ERROR: ' + e
			end
		end
	end

end

def create_zip(filename, root, excludes=/^$/)
  File.delete(filename) if File.exists? filename
  Zip::ZipFile.open(filename, Zip::ZipFile::CREATE) do |zip|
    Find.find(root) do |path|
	  next if path =~ excludes
	  
	  zip_path = path.gsub(root, '')
	  zip.add(zip_path, path)
	end
  end
end

def docu(dll_name)
	FileUtils.rm_r('output') if File.exists? 'output'
	
	docu_exe = "tools/docu/docu.exe"
	
	`#{docu_exe} #{dll_name}`
end