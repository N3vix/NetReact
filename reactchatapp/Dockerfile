FROM node:21

WORKDIR /reactchatapp

COPY public/ /reactchatapp/public
COPY src/ /reactchatapp/src
COPY package.json /reactchatapp/
COPY .env /reactchatapp/

RUN npm install

ARG REACT_APP_API_ADDRESS
ENV REACT_APP_API_ADDRESS $REACT_APP_API_ADDRESS

CMD ["npm", "start"]

# RUN npm run build

# FROM nginx
# COPY --from=ui-builder /home/ui/build /usr/share/nginx/html
# CMD ["nginx", "-g", "daemon off;"]