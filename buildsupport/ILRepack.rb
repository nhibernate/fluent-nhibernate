load "#{File.dirname(__FILE__)}/platform.rb"
class ILRepack

  def initialize(attributes)

    out = attributes.fetch(:out, '')
    lib = attributes.fetch(:lib, nil)
    target = attributes.fetch(:target, 'library')
    targetplatform = attributes.fetch(:targetplatform, "v4")
    internalize = attributes.fetch(:internalize, true)
    debugsymbols = attributes.fetch(:debugsymbols, false)
    union = attributes.fetch(:union, false)

    params = []
    params << "/out:#{out}"
    params << "/lib:#{lib}" unless lib.nil?
    params << "/target:#{target}" unless target.nil?
    params << "/targetplatform:#{targetplatform}" unless targetplatform.nil?
    params << "/internalize" if internalize
    params << "/ndebug" unless debugsymbols
    params << "/union" if union

    merge_exe = "#{File.dirname(__FILE__)}/ILMerge.exe"
    @cmd = "#{merge_exe} #{params.join(' ')}"
  end

  def merge(params)

    if Platform.is_nix
      puts "Merging is *currently* not supported on Mono. Skipping merge..."
      return
    end

    src = params.fetch(:lib, '')
    refs = params.fetch(:refs, []).map {|f| File.join(src, f)}
    sh Platform.runtime("#{@cmd} #{refs.join(' ')}")
  end
end
