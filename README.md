# FolioWatcher

Docker image to periodically check the **Folio Society** website for new additions to the **Last Chance to Buy** page.

## Docker

### Build Dockerfile (using Command Prompt)

- docker build --tag foliowatcher .
- docker save foliowatcher:latest | gzip > foliowatcher.tar.gz

### Add Docker Image to Synology NAS

- Docker -> Image -> Add -> Add From File -> Select foliowatcher.tar.gz

### Create Docker Container on Synology NAS

- Docker -> Container -> Create -> foliowatcher:latest
- Use the same network as the Docker Host
- Map volume /App/Config (Contains Settings.json, don't forget to enter user data!)
- Add environment variable TZ: Europe/Brussels
