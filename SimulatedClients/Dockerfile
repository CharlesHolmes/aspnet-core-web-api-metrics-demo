FROM grafana/k6
COPY simulate_load.js .
COPY run.sh .
ENTRYPOINT ["/bin/sh", "run.sh"]