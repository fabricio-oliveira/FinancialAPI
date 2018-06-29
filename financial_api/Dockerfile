FROM microsoft/dotnet:2.1-sdk

WORKDIR /app

COPY $SOURCE .

RUN chmod +x app.sh

EXPOSE 8080/tcp

ENV ASPNETCORE_URLS http://+:8080
CMD ["/app/app.sh"]
