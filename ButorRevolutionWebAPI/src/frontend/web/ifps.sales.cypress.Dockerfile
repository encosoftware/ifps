FROM cypress/included:3.4.0 as cypress
COPY ["package.json", "yarn.lock", "cypress.json", "tsconfig.json", "reporter-config.json", "/"]
COPY ./cypress/ /cypress/

RUN npm i 

CMD ["npx", "cypress", "run"]


