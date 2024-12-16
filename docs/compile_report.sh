pandoc --filter pandoc-plantuml --output=report.pdf ./report.md --pdf-engine=xelatex
xdg-open ./report.pdf
