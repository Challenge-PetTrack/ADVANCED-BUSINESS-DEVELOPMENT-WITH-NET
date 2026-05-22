# 🐾 PetTrack API — ASP.NET Core

> Sistema operacional da saúde contínua do pet — backend em ASP.NET Core 8 conectado ao Oracle DB, desenvolvido para o Challenge FIAP 2026 em parceria com a **Clyvo Vet**.

---

## 👥 Equipe

| Nome | RM |
|---|---|
| Gabriel Sbrana Campos | RM 565849 |
| Moisés Waidemann | RM 563719 |
| Thiago Rodrigues da Mota | RM 563765 |
| Richard Freitas | RM 566127 |

**Turma:** 2TDS — Fevereiro  
**Disciplina:** Advanced Business Development with .NET  
**Instituição:** FIAP

---

## 📌 Descrição do Projeto

O **PetTrack** é uma plataforma de saúde contínua para pets que conecta tutores, animais e clínicas veterinárias em um único ecossistema digital. A proposta nasce do challenge da **Clyvo Vet**, que busca digitalizar e centralizar a jornada de saúde do animal desde o cadastro até o monitoramento em tempo real via collar IoT.

Esta API em ASP.NET Core 8 é a implementação do backend em .NET, compartilhando o mesmo banco Oracle já utilizado pela API Java Spring Boot do projeto, garantindo consistência de dados entre ambas as implementações.

---

## 🏗️ Arquitetura

```
PetTrack.API/
├── Controllers/         → Camada HTTP — endpoints REST
├── Models/              → Entidades mapeadas pelo EF Core (Oracle)
├── DTOs/
│   ├── Request/         → Payloads de entrada (POST, PUT)
│   └── Response/        → Payloads de saída (GET)
├── Repositories/        → Interface + Implementação (acesso a dados)
├── Services/            → Lógica de negócio
├── Data/                → DbContext + configuração EF Core
└── Program.cs           → Startup, DI, Swagger/Scalar
```

**Stack:**
- ASP.NET Core 8 (Controllers)
- Entity Framework Core 8 + Oracle Provider
- Oracle DB (oracle.fiap.com.br)
- Swagger / Scalar (documentação OpenAPI)
- DTOs separados das entidades
- Padrão REST

---

## 🗄️ Banco de Dados

Banco Oracle compartilhado com a API Java. As tabelas já existem — **não são recriadas pelo .NET**.

| Tabela | Descrição |
|---|---|
| `TB_TUTOR` | Responsável pelo pet |
| `TB_PET` | Animal cadastrado |
| `TB_CLINICA` | Clínica veterinária |
| `TB_EVENTO_CLINICO` | Consultas e eventos clínicos |
| `TB_PROTOCOLO_PREVENTIVO` | Protocolos de vacinação/vermifugação |
| `TB_MEDICAMENTO` | Medicamentos prescritos |
| `TB_ADESAO_MEDICAMENTO` | Adesão do tutor ao medicamento |
| `TB_NOTIFICACAO` | Notificações enviadas ao tutor |
| `TB_SCORE_HISTORICO` | Histórico de score de saúde |
| `TB_BCS_HISTORICO` | Body Condition Score histórico |
| `TB_COLLAR_LEITURA` | Leituras do collar IoT |
| `TB_ALERTA` | Alertas gerados pelo sistema |

> Todas as PKs utilizam **SEQUENCE** do Oracle (não IDENTITY).

---

## 🔗 Rotas da API

### Tutores `/api/tutores`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/tutores` | Lista todos os tutores | 200 |
| GET | `/api/tutores/{id}` | Busca tutor por ID | 200 / 404 |
| GET | `/api/tutores/{id}/pets` | Lista pets de um tutor | 200 / 404 |
| POST | `/api/tutores` | Cria um novo tutor | 201 / 400 |
| PUT | `/api/tutores/{id}` | Atualiza dados do tutor | 200 / 404 |
| DELETE | `/api/tutores/{id}` | Remove um tutor | 204 / 404 |

### Pets `/api/pets`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/pets` | Lista todos os pets | 200 |
| GET | `/api/pets/{id}` | Busca pet por ID | 200 / 404 |
| GET | `/api/pets/{id}/eventos` | Eventos clínicos do pet | 200 / 404 |
| GET | `/api/pets/{id}/alertas` | Alertas do pet | 200 / 404 |
| POST | `/api/pets` | Cadastra um novo pet | 201 / 400 |
| PUT | `/api/pets/{id}` | Atualiza dados do pet | 200 / 404 |
| DELETE | `/api/pets/{id}` | Remove um pet | 204 / 404 |

### Clínicas `/api/clinicas`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/clinicas` | Lista todas as clínicas | 200 |
| GET | `/api/clinicas/{id}` | Busca clínica por ID | 200 / 404 |
| GET | `/api/clinicas/{id}/eventos` | Eventos da clínica | 200 / 404 |
| POST | `/api/clinicas` | Cadastra clínica | 201 / 400 |
| PUT | `/api/clinicas/{id}` | Atualiza clínica | 200 / 404 |
| DELETE | `/api/clinicas/{id}` | Remove clínica | 204 / 404 |

### Eventos Clínicos `/api/eventos`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/eventos` | Lista todos os eventos | 200 |
| GET | `/api/eventos/{id}` | Busca evento por ID | 200 / 404 |
| GET | `/api/eventos/pet/{petId}` | Eventos por pet | 200 / 404 |
| POST | `/api/eventos` | Registra evento clínico | 201 / 400 |
| PUT | `/api/eventos/{id}` | Atualiza evento | 200 / 404 |
| DELETE | `/api/eventos/{id}` | Remove evento | 204 / 404 |

### Collar Leituras `/api/collar-leituras`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/collar-leituras` | Lista todas as leituras | 200 |
| GET | `/api/collar-leituras/{id}` | Busca leitura por ID | 200 / 404 |
| GET | `/api/collar-leituras/pet/{petId}` | Leituras por pet | 200 / 404 |
| POST | `/api/collar-leituras` | Registra leitura | 201 / 400 |
| DELETE | `/api/collar-leituras/{id}` | Remove leitura | 204 / 404 |

### Alertas `/api/alertas`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/alertas` | Lista todos os alertas | 200 |
| GET | `/api/alertas/{id}` | Busca alerta por ID | 200 / 404 |
| GET | `/api/alertas/pet/{petId}` | Alertas por pet | 200 / 404 |
| POST | `/api/alertas` | Cria alerta | 201 / 400 |
| PUT | `/api/alertas/{id}` | Atualiza alerta | 200 / 404 |
| DELETE | `/api/alertas/{id}` | Remove alerta | 204 / 404 |

---

## 🚀 Como Rodar (How To)

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso à rede da FIAP (VPN ou presencial para o Oracle)
- Git

### 1. Clonar o repositório

```bash
git clone https://github.com/SEU_USUARIO/pettrack-dotnet.git
cd pettrack-dotnet
```

### 2. Configurar a connection string

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=rm563719;Password=111206;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}
```

### 3. Restaurar dependências

```bash
dotnet restore
```

### 4. Aplicar migrations (opcional — banco já existe)

```bash
dotnet ef database update
```

### 5. Rodar a aplicação

```bash
dotnet run --project PetTrack.API
```

A API estará disponível em:
- `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`
- Scalar UI: `http://localhost:5000/scalar`

---

## 📦 Pacotes NuGet

```xml
<PackageReference Include="Oracle.EntityFrameworkCore" Version="8.21.121" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```

---

## 📄 Documentação

A documentação completa dos endpoints está disponível via Swagger/Scalar ao rodar a aplicação localmente.

Coleção Insomnia/Postman: disponível na pasta `/docs` do repositório.

---

## 🎯 Benefícios para o Negócio

- **Tutores** têm visibilidade contínua da saúde do seu pet, com alertas proativos
- **Clínicas** ganham histórico clínico centralizado e comunicação direta com tutores
- **Clyvo Vet** aumenta engajamento e retenção de clientes com dados de saúde em tempo real via collar IoT
- **Interoperabilidade** garantida pelo banco Oracle compartilhado com a API Java

---

## 📁 Estrutura do Repositório

```
pettrack-dotnet/
├── PetTrack.API/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   │   ├── Request/
│   │   └── Response/
│   ├── Repositories/
│   ├── Services/
│   ├── Data/
│   ├── appsettings.json
│   └── Program.cs
├── docs/
│   └── PetTrack.postman_collection.json
├── .gitignore
└── README.md
```

---

> Projeto desenvolvido para o **Challenge FIAP 2026** — Clyvo Vet × 2TDS Fevereiro
