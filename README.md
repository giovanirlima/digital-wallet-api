# 🏧 Digital Wallet Challenge

API desenvolvida com **.NET 9** para gerenciamento de usuários, carteiras digitais e transações financeiras. O projeto inclui autenticação com JWT, validações robustas e persistência de dados com Entity Framework e PostgreSQL.

---

## ✔ Tecnologias Utilizadas

- **.NET 9** — Framework principal da aplicação  
- **Swagger** — Documentação interativa da API  
- **AutoMapper** — Mapeamento de objetos  
- **FluentValidation** — Validações robustas  
- **MediatR** — Implementação do padrão Mediator  
- **JWT** — Autenticação com Json Web Token  
- **Entity Framework Core** — ORM para PostgreSQL  
- **PostgreSQL** — Banco de dados principal  
- **RabbitMQ** — Processamento assíncrono de transações  
- **XUnit + Moq** — Testes unitários e mocks  
- **Docker** — Containerização e ambiente de desenvolvimento  

---

## 📝 Funcionalidades Principais

### 👤 Gestão de Usuários

- Cadastro, edição e exclusão de usuários  
- Autenticação via JWT  

### 💰 Operações Financeiras

- Consulta de saldo  
- Depósito na carteira  
- Saque de valores  
- Transferência entre usuários  
- Histórico de transações (enviadas/recebidas)  

---

## 🏗️ Arquitetura e Padrões

- **Clean Architecture** — Separação de camadas e responsabilidades  
- **CQRS** — Comandos e consultas separados  
- **Repository Pattern** — Abstração de acesso a dados  
- **SOLID** — Princípios de design orientado a objetos  

---

## 🚀 Como Executar o Projeto

### 🔧 Configuração Local

**Pré-requisitos:**

- .NET 9 SDK  
- PostgreSQL 16 instalado ou uso via Docker  

### 📦 Banco de Dados

Com Docker:

```bash
docker run -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:16
```

Execute os scripts de migração em:  
`deployment/scripts/v1.0.0`

### ▶️ Executando a Aplicação

Inicie os seguintes projetos:

- `Digital.Wallet.Api` — API principal  
- `Digital.Wallet.Application.Consumer` — Processador de transações

### 🐳 Docker Compose (Recomendado)

Para executar de forma automatica, navega até a raiz do projeto e execute o comando:
```bash
docker-compose up --build
```

Para parar os servicos:
```bash
ctrl + c
```

Para acessar o swagger da aplicação:
Acesse em: [http://localhost:7294/swagger](http://localhost:7294/swagger)  
Para encerrar: `Ctrl + C`

---

## 🧪 Testes

### Testes Unitários

Utilize o comando no console, para uma melhor experiência:
```bash
dotnet test
```

- Escritos com **XUnit**  
- Utilização de **Moq** para mocks  

---

## 🔐 Credenciais de Teste

Usuários disponíveis: `user1@teste.com` até `user10@teste.com`

Exemplo:

```json
{
  "email": "user1@teste.com",
  "password": "SenhaForte12344$$"
}
```

---

## 📌 Observações Importantes

- O processamento de transações é **assíncrono via RabbitMQ**  
- Todas as requisições (exceto login) exigem **autenticação JWT**  
- O container Docker já inclui:
  - PostgreSQL  
  - RabbitMQ  
  - Aplicação principal  
  - Consumer de transações  

---

## 📄 Documentação

- Acesse `/swagger` para explorar os endpoints da API  
- Schema do banco de dados disponível em: `deployment/schema`
