# To use the following script you need to have the pandoc-plantuml-filter pip program installed, and pandoc, with latex packages
pandoc -f markdown-implicit_figures --filter pandoc-plantuml --output=report.pdf ./report.md --pdf-engine=xelatex
xdg-open ./report.pdf
