namespace :docs do

  DOC_DIR="docs"
  DOCS_BUILD_DIR="#{DOC_DIR}/build"
  DOC_SOURCE="#{DOC_DIR}/source"
  SPHINXOPTS=ENV['SPHINXOPTS']
  ALLSPHINXOPTS="#{SPHINXOPTS} -d #{DOCS_BUILD_DIR}/doctrees #{DOC_SOURCE}"

  def run_sphinx(args)
    sh %{sphinx-build #{args}} do |ok, res|
      if ! ok
        p res
      end
    end
  end

  desc "Cleans the built documentation directories"
  task :clean do
    rm_rf "#{DOCS_BUILD_DIR}"
  end

  desc "Make standalone HTML files"
  task :html do
    run_sphinx "-b html #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/html"
  end

  #desc "to make HTML files named index.html in directories"
  #task :dirhtml do
  #end

  desc "Make a single large HTML file"
  task :singlehtml do
    run_sphinx "-b singlehtml #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/singlehtml"
  end

  #desc "to make pickle files"
  #task :pickle do
  #end

  desc "Make JSON files"
  task :json do
    run_sphinx "-b json #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/json"
  end

  #desc "to make HTML files and a HTML help project"
  #task :htmlhelp do
  #end

  #desc "to make HTML files and a qthelp project"
  #task :qthelp do
  #end

  #desc "to make HTML files and a Devhelp project"
  #task :devhelp do
  #end

  desc "Make an epub"
  task :epub do
    run_sphinx "-b epub #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/epub"
  end

  desc "Make LaTeX files, you can set PAPER=a4 or PAPER=letter"
  task :latex do
    run_sphinx "-b latex #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/latex"
  end

  desc "Make LaTeX files and run them through pdflatex"
  task :latexpdf do
    run_sphinx "-b latex #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/latex"
    puts "Running LaTeX files through pdflatex..."
    sh "make -C $(DOCS_BUILD_DIR)/latex all-pdf"
  end

  desc "Make text files"
  task :text do
    run_sphinx "-b text #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/text"
  end

  #desc "to make manual pages"
  #task :man do
  #end

  desc "Make an overview of all changed/added/deprecated items"
  task :changes do
    run_sphinx "-b changes #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/changes"
  end

  desc "Check all external links for integrity"
  task :linkcheck do
    run_sphinx "-b linkcheck #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/linkcheck"
  end

  desc "Run all doctests embedded in the documentation (if enabled)"
  task :doctest do
    run_sphinx "-b doctest #{ALLSPHINXOPTS} #{DOCS_BUILD_DIR}/doctest"
  end
end
