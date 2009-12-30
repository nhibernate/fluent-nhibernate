require 'albacore'
require 'albacore/support/albacore_helper'
require 'rake/tasklib'

# zip task modification - allow exclusions
class ZipDirectory
  attr_accessor :exclusions
  
  def reject_file(f)
    f == zip_name || is_excluded(f)
  end
  
  private
  def is_excluded(f)
    (@exclusions || []).any? do |e|
      return true if e.respond_to? '~' and f =~ e
      e == f
    end
  end
end

# docu task - remove once in Albacore master

class Docu
  include RunCommand
  
  attr_accessor :path_to_command, :assemblies, :xml_files, :output_location
  
  def initialize(path_to_command='docu.exe')
    super()
    @path_to_command = path_to_command
    @assemblies = []
    @xml_files = []
    @output_location = ""
  end
  
  def execute
    if @assemblies.empty?
      fail_with_message 'Docu Failed. No assemblies specified'
      return
    end
  
    command_params = get_command_parameters
    success = run_command 'Docu', command_params.join(' ')
  
    fail_with_message 'Docu Failed. See Build Log For Detail' unless success
  end
  
  private
  def get_command_parameters
    command_params = []
    command_params << @assemblies.join(' ') unless @assemblies.nil?
    command_params << @xml_files.join(' ') unless @xml_files.nil?
    command_params << " --output=\"#{@output_location}\" " unless @output_location.empty?
    command_params
  end
end

def docu(name=:docu, *args, &block)
  Albacore::DocuTask.new(name, *args, &block)
end

module Albacore
  class DocuTask < Albacore::AlbacoreTask
    def execute(task_args)
	  @docu = Docu.new
	  @block.call(@docu, *task_args) unless @block.nil?
	  @docu.execute
	  fail if @docu.failed
	end
  end
end