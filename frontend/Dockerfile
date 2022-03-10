FROM nginx:stable-alpine

WORKDIR /etc/nginx

COPY ./default.conf ./conf.d/default.conf

COPY build /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
