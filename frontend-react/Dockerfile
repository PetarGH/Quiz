FROM node:12.2.0-alpine

WORKDIR /app
COPY . .
RUN chmod +x node_modules/.bin

RUN npm install react-scripts --save

ENTRYPOINT ["npm", "start"]
