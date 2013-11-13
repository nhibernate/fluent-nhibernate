require 'erb'

class NUnitRunner
	include FileTest

	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, '')
		@compileTarget = paths.fetch(:compilemode, 'debug')
	
		@nunitExe = Nuget.tool("NUnit", "nunit-console#{(@compilePlatform.empty? ? '' : "-#{@compilePlatform}")}.exe") + Platform.switch("nothread")
	end
	
	def executeTests(assemblies)
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{@compileTarget}/#{assem}.dll")
			sh Platform.runtime("#{@nunitExe} -xml=#{@resultsDir}/#{assem}-TestResults.xml \"#{file}\"")
		end
	end
end

class MSBuildRunner
	def self.compile(attributes)
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    solutionFile = attributes[:solutionfile]
	    
	    attributes[:projFile] = solutionFile
	    attributes[:properties] ||= []
	    attributes[:properties] << "Configuration=#{compileTarget}"
	    attributes[:extraSwitches] = ["v:m", "t:rebuild"]
		  attributes[:extraSwitches] << "maxcpucount:2" unless Platform.is_nix

      self.runProjFile(attributes);
	end
	
	def self.runProjFile(attributes)
		version = attributes.fetch(:clrversion, 'v4.0.30319')
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    projFile = attributes[:projFile]
		
    if Platform.is_nix
      msbuildFile = `which xbuild`.chop
      attributes[:properties] << "TargetFrameworkProfile="""""
    else
		  frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', version)
		  msbuildFile = File.join(frameworkDir, 'msbuild.exe')
   end

    properties = attributes.fetch(:properties, [])
		
		switchesValue = ""
		
		properties.each do |prop|
			switchesValue += " /property:#{prop}"
		end	
		
		extraSwitches = attributes.fetch(:extraSwitches, [])	
		
		extraSwitches.each do |switch|
			switchesValue += " /#{switch}"
		end	
		
		targets = attributes.fetch(:targets, [])
		targetsValue = ""
		targets.each do |target|
			targetsValue += " /t:#{target}"
		end
		
		sh "#{msbuildFile} #{projFile} #{targetsValue} #{switchesValue}"
	end
end

class AspNetCompilerRunner
	def self.compile(attributes)
		if Platform.is_nix
      puts "Skipping AspNet compilation. Not supported by Mono."
      return
    end
		webPhysDir = attributes.fetch(:webPhysDir, '')
		webVirDir = attributes.fetch(:webVirDir, 'This_Value_Is_Not_Used')
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', 'v4.0.30319')
		aspNetCompiler = File.join(frameworkDir, 'aspnet_compiler.exe')

		sh "#{aspNetCompiler} -nologo -errorstack -c -p #{webPhysDir} -v #{webVirDir}"
	end
end

class AsmInfoBuilder
	attr_reader :buildnumber

	def initialize(baseVersion, properties)
		@properties = properties;
		
		@buildnumber = baseVersion + (ENV["CCNetLabel"].nil? ? '0' : ENV["CCNetLabel"].to_s)
		@properties['Version'] = @properties['InformationalVersion'] = buildnumber;
	end


	
	def write(file)
		template = %q{
using System;
using System.Reflection;
using System.Runtime.InteropServices;

<% @properties.each {|k, v| %>
[assembly: Assembly<%=k%>Attribute("<%=v%>")]
<% } %>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open(file, 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end
