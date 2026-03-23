# School Manager API 🏫

Um sistema de gestão escolar focado em performance e arquitetura limpa, construído para gerenciar operações acadêmicas de forma eficiente.

## 🚀 Tecnologias Utilizadas
* **Back-End:** C# e ASP.NET Core MVC / API
* **Banco de Dados:** Entity Framework Core & SQL Server (ou MySQL/SQLite)
* **Arquitetura:** Padrões MVC, injeção de dependência e princípios de Clean Code.

## ⚙️ Funcionalidades
* Modelagem de domínio para entidades escolares (Alunos, Professores, Turmas).
* Operações completas de CRUD com validação de regras de negócio.
* Estruturação de rotas RESTful para integração futura com interfaces Front-end.

## 🛠️ Como rodar o projeto localmente
1. Clone este repositório: `git clone https://github.com/Zoberobe/school-manager.git`
2. Abra a solução no Visual Studio ou VS Code.
3. Restaure os pacotes NuGet e atualize o banco de dados via Entity Framework (`Update-Database`).
4. Execute a aplicação (F5 ou `dotnet run`).
