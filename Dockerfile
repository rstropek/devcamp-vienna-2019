FROM ubuntu:bionic

RUN apt-get update && \
    apt-get -y -q install apt-utils dialog

RUN apt-get -y -q install \
        apt-transport-https \
        ca-certificates \
        curl \
        gnupg-agent \
        git \
        gpg \
        jq \
        lsb-release \
        software-properties-common

RUN curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | \
    tee /etc/apt/trusted.gpg.d/microsoft.asc.gpg > /dev/null && \
    AZ_REPO=$(lsb_release -cs) && \
    echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | \
    tee /etc/apt/sources.list.d/azure-cli.list

RUN curl -fsSL https://download.docker.com/linux/ubuntu/gpg | apt-key add - && \
    add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"

RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key add - && \
    echo "deb https://apt.kubernetes.io/ kubernetes-xenial main" | tee -a /etc/apt/sources.list.d/kubernetes.list

RUN apt-get update
RUN apt-get -y -q install \
    azure-cli \
    docker-ce-cli \
    kubectl

RUN mkdir /devcamp
WORKDIR /devcamp
COPY ./HotelsApi /devcamp/HotelsApi

CMD [ "/bin/bash" ]
