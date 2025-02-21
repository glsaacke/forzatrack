FROM node:18-alpine

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install

EXPOSE 5173

CMD ["npm", "run", "dev"]

#Docker cli commands
#NEW IMAGE: docker build -t <tag name> .
#NEW CONTAINER: docker run -p <port>:<port> <image name>
#NEW CONTAINER WITH FILE VOLUME: docker run -p <port>:<port> -v "$(pwd):/app" -v /app/node_modules <image name>