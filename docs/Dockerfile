FROM fedora:40
RUN dnf update -y && \
    dnf install -y \
        pandoc \
        plantuml \
        texlive-scheme-basic \
        graphviz \
        python3 \
        pip \
        librsvg2-tools && \
    pip install pandoc-plantuml-filter
COPY ./ ./
ENTRYPOINT ["pandoc", "--filter", "pandoc-plantuml"]
