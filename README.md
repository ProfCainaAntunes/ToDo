# ToDo API — ASP.NET Core (.NET 10)

API Web simples para gerenciamento de tarefas (ToDo) desenvolvida com ASP.NET Core (.NET 10), xUnit, Docker e GitHub Actions.

O projeto foi criado com foco em:
- testes automatizados;
- integração contínua (CI);
- entrega contínua (CD);
- containerização com Docker;
- garantia automatizada de qualidade.

---

# Tecnologias Utilizadas

- ASP.NET Core (.NET 10)
- xUnit
- Swagger / OpenAPI
- Docker
- GitHub Actions

---

# Estrutura do Projeto

```text
ToDo/
├── ToDo.slnx
├── global.json
├── Dockerfile
├── .dockerignore
├── .github/
│   └── workflows/
│       └── dotnet.yml
│
├── src/
│   └── ToDo.Api/
│       ├── Controllers/
│       ├── Services/
│       ├── Models/
│       ├── DTOs/
│       ├── Program.cs
│       └── ToDo.Api.csproj
│
└── tests/
    └── ToDo.Tests/
        ├── Services/
        ├── Controllers/
        └── ToDo.Tests.csproj
```

---

# Funcionalidades

A API disponibiliza os seguintes endpoints:

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/tasks` | Lista todas as tarefas |
| POST | `/tasks` | Cria uma nova tarefa |
| PUT | `/tasks/{id}/complete` | Marca uma tarefa como concluída |
| DELETE | `/tasks/{id}` | Remove uma tarefa |

---

# Modelo de Dados

## TodoItem

```json
{
  "id": "guid",
  "title": "Estudar CI/CD",
  "isCompleted": false,
  "priority": "High"
}
```

## Priority

Valores disponíveis:

- Low
- Medium
- High

---

# Validações

A API valida:
- título obrigatório;
- título não vazio;
- IDs inexistentes para conclusão/exclusão.

---

# Testes Automatizados

O projeto utiliza xUnit com testes reais sem mocks de terceiros.

Os testes cobrem:
- criação válida;
- criação inválida;
- listagem;
- conclusão;
- exclusão;
- retornos HTTP esperados.

## Executar testes

```bash
dotnet test
```

---

# Requisitos

- .NET SDK 10
- Docker Desktop (opcional)

---

# Executando o Projeto Localmente

## 1. Clonar o repositório

```bash
git clone <URL_DO_REPOSITORIO>
```

---

## 2. Acessar a pasta do projeto

```bash
cd ToDo
```

---

## 3. Restaurar dependências

```bash
dotnet restore
```

---

## 4. Executar a aplicação

```bash
dotnet run --project src/ToDo.Api/ToDo.Api.csproj
```

---

# Swagger / OpenAPI

Após iniciar a aplicação, acessar:

```text
http://localhost:5000/swagger
```

ou a URL exibida no terminal.

---

# Executando com Docker

## Build da imagem

```bash
docker build -t todo-api-net10 .
```

---

## Executar container

```bash
docker run -d -p 8080:8080 --name todo-api-container todo-api-net10
```

---

## Acessar Swagger

```text
http://localhost:8080/swagger
```

---

# Pipeline CI/CD

O projeto possui pipeline automatizado utilizando GitHub Actions.

O workflow executa:
- restore;
- build;
- testes automatizados;
- build Docker.

Arquivo:

```text
.github/workflows/dotnet.yml
```

---

# Estrutura Arquitetural

A aplicação utiliza:
- Controllers;
- Services;
- DTOs;
- Injeção de Dependência;
- armazenamento em memória utilizando singleton.

Não utiliza:
- banco de dados;
- Entity Framework;
- ORM.

---

# Serviço em Memória

As tarefas são armazenadas em memória utilizando:
- `List<TodoItem>`;
- singleton thread-safe;
- locks para concorrência.

---

# Executando Manualmente os Endpoints

## Listar tarefas

```http
GET /tasks
```

---

## Criar tarefa

```http
POST /tasks
Content-Type: application/json
```

Body:

```json
{
  "title": "Estudar Docker",
  "priority": "High"
}
```

---

## Concluir tarefa

```http
PUT /tasks/{id}/complete
```

---

## Excluir tarefa

```http
DELETE /tasks/{id}
```

---

# Build da Solução

```bash
dotnet build
```

---

# Formato da Solução

O projeto utiliza o novo formato `.slnx` introduzido no .NET 10.

---

# Objetivo Educacional

Este projeto foi desenvolvido para demonstração prática de:
- integração contínua;
- entrega contínua;
- testes automatizados;
- pipelines CI/CD;
- garantia automatizada de qualidade em aplicações Web API.