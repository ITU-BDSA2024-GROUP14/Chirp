pandoc -f markdown-implicit_figures --filter pandoc-plantuml --output=report.pdf ./report.md --pdf-engine=xelatex
xdg-open ./report.pdf
