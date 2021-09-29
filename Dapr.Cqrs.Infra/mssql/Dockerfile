FROM mcr.microsoft.com/mssql/server:2019-latest

USER root

# Copy initialization scripts
COPY init.sql init.sql
COPY init.sh init.sh
COPY entrypoint.sh entrypoint.sh

# Grant permissions for the run-initialization script to be executable
RUN chmod +x init.sh
RUN chmod +x entrypoint.sh

# Run Microsoft SQl Server and initialization script (at the same time)
CMD /bin/bash ./entrypoint.sh
