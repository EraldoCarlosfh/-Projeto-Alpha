
![img](https://github.com/EraldoCarlosfh/Projeto-Alpha/blob/main/Alpha-Api.jpg)

# 	API REST PROJETO ALPHA - ERALDO CARLOS

## Índice

1. [Sobre o projeto]

2. [Tecnologias utilizadas]

### 1. Sobre o projeto

O objetivo deste projeto foi a criação de uma API Rest para o armazenamento de dados de um produto, e o desenvolvimento frontend com Vue.js para listagem, cadastro, edição e remoção dos produtos (CRUD), também foi implementado todo um sistema de autenticação com JWT no backend e aplicação proteção de rotas no frontend, o sistema conta com as telas da área não logado de cadastro de usuário e login, para o sistema usuário precisa primeiro efetuar cadastro.

### 2.Tecnologias Utilizadas e Procedimentos

Desenvolvimento Frontend - Vue.Js, TailWindcss, PrimeVue, Axios.
1 - Rodar o comando npm i ou npm install para instalação das dependências do projeto contidas no package.json.
2 - Após finalizado a instação das depedências inicializar o projeto com o comando npm run dev.

Docker - Utilizada imagem docker do banco de dados SqlServer 2022, abaixo comandos nescessários para upload da imagem docker e sua inicialização.
1 - Download imagem SqlServer docker => docker pull mcr.microsoft.com/mssql/server:2022-latest
2- Configurar senha do banco, o comando abaixo irá definir a senha do banco de dados com e inicializar o container, para adicionar uma senha de sua preferência 
altere o valor de YourStrong@Passw0rd para a senha desejada.
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sqlserver-container -d mcr.microsoft.com/mssql/server:2022-latest

Dica: Para acesso as informações do banco de dados SqlServer rodando com Docker intale o Azure Data Studio e efetue a conexão com os dados da sua
ConnectionString

Desenvolvimento Backend - .NET CORE 8.0, Entity Framework Core, Fluent Validation, MediatR, Swagger.
1 - Após inicialização de container docker com imagem do SqlServer inicializar projeto com ISS Express Devlopment
2 - Rodar comando Add-Migration Initial, no Console do Gerenciador de Pacotes o projeto padrão será o Aplha.Data
3 - Após geração da migration rodar comando Update-Database, após finalizado será criado o banco myDataBase em sua instância Docker
4 - Em Appsetting.Development alterar senha do banco para senha definido por você na configuração do container docker com SqlServe
"DefaultConnection": "Server=localhost,1433; Database=myDataBase; User Id=sa; Password=SenhaBanco; TrustServerCertificate=true;", 
altera em Password a SenhaBanco para senha que você definiu.

Finalizado todo este processo volte para a aplicação frontend cadastre um usuário e logue o sistema.

![img](https://github.com/EraldoCarlosfh/Projeto-Alpha/blob/main/Alpha-Front-3.jpg)
![img](https://github.com/EraldoCarlosfh/Projeto-Alpha/blob/main/Alpha-Front-4.jpg)
![img](https://github.com/EraldoCarlosfh/Projeto-Alpha/blob/main/Alpha-Front-1.jpg)
![img](https://github.com/EraldoCarlosfh/Projeto-Alpha/blob/main/Alpha-Front-2.jpg)


