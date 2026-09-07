# Sistema de Gestão de Consultas UVV

Sistema web desenvolvido em C# com ASP.NET Core MVC para gerenciamento de consultas.

## Tecnologias utilizadas

- C#
- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML
- CSS
- Bootstrap

## Funcionalidades

O sistema possui as seguintes funcionalidades:

- Cadastro de usuários
- Validação dos dados do usuário
- Login e autenticação
- Logout
- Cadastro de consultas
- Visualização das próprias consultas
- Edição de consultas
- Exclusão de consultas
- Controle de acesso às consultas por usuário

## Estrutura do sistema

O projeto utiliza o padrão arquitetural MVC:

- **Models:** representam as entidades `Usuario` e `Consulta`.
- **Views:** responsáveis pela interface do sistema.
- **Controllers:** responsáveis pelas regras de controle e comunicação entre as Views e o banco de dados.
- **Data:** contém o contexto do Entity Framework Core.

## Banco de dados

O sistema utiliza o **SQL Server** com o Entity Framework Core utilizando a abordagem **Code First**.

A conexão com o banco de dados está configurada no arquivo:

`appsettings.json`

A aplicação utiliza a seguinte instância do SQL Server:

`.\\SQLEXPRESS`

O banco de dados utilizado pelo projeto é:

`SistemaConsultasUVV`

## Migrations

Para criar ou atualizar o banco de dados, utilizando o Console do Gerenciador de Pacotes do Visual Studio, execute:

```powershell
Update-Database

link para o vídeo:
https://drive.google.com/file/d/1Qix6GVUA9SFr2qwYIZjrO0fN4uac67m0/view?usp=sharing