# 🐾 PetTrack API — ASP.NET Core & Clyvo Vet (Sprint 3)

> **Sistema Operacional da Saúde Contínua do Pet** — API REST desenvolvida em **ASP.NET Core (.NET 10)** com **Clean Architecture**, conectada ao **Oracle Database**, equipada com **Observabilidade completa (Serilog, Health Checks, OpenTelemetry / Prometheus)** e **Testes Automatizados com padrão AAA (xUnit, Moq, FluentAssertions, WebApplicationFactory)** para o **Challenge FIAP 2026** em parceria com a **Clyvo Vet**.

---

## 👥 Integrantes da Equipe

| Nome Completo | RM | Turma |
|---|---|---|
| **Gabriel Sbrana Campos** | RM 565849 | 2TDSPF |
| **Moisés Waidemann** | RM 563719 | 2TDSPF |
| **Thiago Rodrigues da Mota** | RM 563765 | 2TDSPF |
| **Richard Freitas** | RM 566127 | 2TDSPF |

- **Instituição:** FIAP — Faculdade de Informática e Administração Paulista
- **Curso:** Análise e Desenvolvimento de Sistemas (2º Ano ADS - Turma 2TDSPF)
- **Disciplina:** Advanced Business Development with .NET
- **Parceiro de Negócio:** Clyvo Vet

---

## 📋 Sumário
1. [Visão Geral do Projeto](#-visão-geral-do-projeto)
2. [Arquitetura da Solução](#-arquitetura-da-solução)
3. [Observabilidade e Monitoramento](#-observabilidade-e-monitoramento)
   - [Health Checks (`/health`, `/health/live`, `/health/ready`)](#1-health-checks)
   - [Structured Logging com Serilog](#2-structured-logging-com-serilog)
   - [Distributed Tracing & Métricas Prometheus via OpenTelemetry](#3-distributed-tracing--métricas-prometheus-via-opentelemetry)
4. [Testes Automatizados com Padrão AAA](#-testes-automatizados-com-padrão-aaa)
   - [PetTrack.UnitTests (xUnit + Moq + FluentAssertions)](#1-testes-unitários-pettrackunittests)
   - [PetTrack.IntegrationTests (WebApplicationFactory + InMemory)](#2-testes-de-integração-pettrackintegrationtests)
   - [Como Executar a Suíte de Testes](#3-como-executar-os-testes)
5. [Endpoints da API REST & Swagger UI](#-endpoints-da-api-rest--swagger-ui)
6. [Instruções de Instalação e Execução](#-instruções-de-instalação-e-execução)
7. [Tecnologias e Pacotes Utilizados](#-tecnologias-e-pacotes-utilizados)

---

## 📌 Visão Geral do Projeto

O **PetTrack** é um ecossistema digital para acompanhamento contínuo da saúde preventiva de cães e gatos. Ele integra:
- Cadastro e gestão de tutores, animais e clínicas veterinárias;
- Histórico clínico, prescrição e controle de adesão a medicamentos;
- Monitoramento telemétrico e corporal contínuo via coleiras inteligentes (**Colares IoT Clyvo**);
- Detecção preventiva de anomalias (febre, sedentarismo, obesidade, falhas de adesão terapêutica) gerando **Alertas** e **Notificações** em tempo real;
- Compatibilidade e consistência transacional com o banco de dados Oracle relacional compartilhado com as demais disciplinas (Mastering Relational and Non-Relational Database e Java Advanced).

---

## 🏗️ Arquitetura da Solução

O projeto adota os princípios da **Clean Architecture** (Arquitetura Limpa) e **Domain-Driven Design (DDD)**:

```
PetTrack/
├── PetTrack.API/                  → Entrypoint HTTP, Controllers, Middleware de Exceção, Swagger, Serilog, OpenTelemetry, Health Checks
├── PetTrack.Application/          → Casos de uso, DTOs (Requests/Responses), Interfaces de Services/Repositories, Métricas customizadas
├── PetTrack.Domain/               → Entidades de Domínio, Invariantes e Regras de Negócio Puras, Enums
├── PetTrack.Infrastructure/       → DbContext EF Core, Mapeamentos Fluent API, Repositórios de Persistência Oracle
├── PetTrack.UnitTests/            → Testes Unitários de Domínio e Serviços (xUnit + Moq + FluentAssertions + Fixtures)
└── PetTrack.IntegrationTests/     → Testes de Integração de API End-to-End (WebApplicationFactory + InMemory DB)
```

### Regras de Dependência:
```
[PetTrack.API] ───► [PetTrack.Application] ───► [PetTrack.Domain]
      │                       ▲
      ▼                       │
[PetTrack.Infrastructure] ────┘
```

---

## 🔭 Observabilidade e Monitoramento

### 1. Health Checks
A aplicação expõe 3 endpoints especializados de integridade com resposta formatada em JSON detalhado (`UIResponseWriter.WriteHealthCheckUIResponse`):

- **`/health`**: Visão completa e consolidada do status de saúde de todos os subsistemas da API.
- **`/health/live`**: *Liveness Probe* do Kubernetes/Cloud para verificar se o processo da API está ativo e respondendo (`self`).
- **`/health/ready`**: *Readiness Probe* para validar se a aplicação está pronta para receber tráfego, testando o banco de dados Oracle (`oracle_db`) e o gateway de telemetria dos colares IoT (`clyvo_iot_service`).

#### Exemplo de Resposta de `/health`:
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "self": {
      "data": {},
      "description": "API PetTrack online e responsiva",
      "duration": "00:00:00.0001234",
      "status": "Healthy",
      "tags": ["live"]
    },
    "oracle_db": {
      "data": {},
      "duration": "00:00:00.0054321",
      "status": "Healthy",
      "tags": ["ready", "db"]
    },
    "clyvo_iot_service": {
      "data": {
        "clyvo_iot_gateway": "Connected",
        "response_time_ms": 12,
        "firmware_supported_version": "v2.4.0"
      },
      "description": "Serviço de telemetria IoT Clyvo operando normalmente.",
      "duration": "00:00:00.0003210",
      "status": "Healthy",
      "tags": ["ready", "integration"]
    }
  }
}
```

---

### 2. Structured Logging com Serilog
A API foi instrumentada com **Serilog**, capturando eventos e logs estruturados em dois destinos simultâneos:
1. **Console**: Colorido com enriquecimento de contexto (`FromLogContext`, timestamp, trace ID);
2. **Arquivo Rotativo Diário**: Gravado em `logs/pettrack-.log` com retenção automática de 7 dias (`rollingInterval: Day`).

#### Exemplo de Formato Estruturado:
```
2026-09-08 20:05:45.123 -03:00 [INF] [d8a7c8164b321a] [PetTrack.API.Controllers.PetsController] HTTP POST /api/pets responded 201 in 14.523 ms
```

---

### 3. Distributed Tracing & Métricas Prometheus via OpenTelemetry
A aplicação utiliza a biblioteca padrão **OpenTelemetry** com exportação nativa no endpoint:
- **`/metrics`**: Endpoint no formato padrão do Prometheus contendo métricas de runtime, ASP.NET Core, HttpClient e métricas de negócio exclusivas da PetTrack:
  - `pettrack_pets_created_total`: Contador acumulado de pets cadastrados;
  - `pettrack_tutores_created_total`: Contador acumulado de tutores cadastrados;
  - `pettrack_alertas_created_total`: Contador acumulado de alertas clínicos/preventivos emitidos;
  - `pettrack_collar_readings_total`: Contador de telemetrias processadas do colar inteligente;
  - `pettrack_adesao_medicamento_total`: Contador de doses administradas/registradas.

---

## 🧪 Testes Automatizados com Padrão AAA

A suíte possui **86 testes automatizados (100% de aprovação)** organizados rigorosamente sob a convenção:
`MetodoTestado_Cenario_ResultadoEsperado` e divididos em **Arrange, Act e Assert**.

```
Aprovado! – Total: 86 testes (71 Unitários + 15 de Integração) – Falhas: 0
```

### 1. Testes Unitários (`PetTrack.UnitTests`)
- **Domínio (`Domain/`)**: Validação de invariantes, regras de negócio e lançamentos de exceções nas entidades `Pet`, `Tutor`, `Alerta`, `AdesaoMedicamento`, `Clinica`, `EventoClinico`, `Medicamento` e `CollarLeitura`.
- **Serviços (`Services/`)**: Casos de uso e regras de serviço testados isoladamente com mocks do **Moq** (`PetServiceTests`, `TutorServiceTests`, `AlertaServiceTests`, `AdesaoMedicamentoServiceTests`, etc.), validando retorno de DTOs, chamadas aos repositórios e tratamento de exceções de chave não encontrada (`KeyNotFoundException`).
- **Fixtures (`Fixtures/PetTrackTestFixtures.cs`)**: Centralização de builders e dados fake reutilizáveis.

### 2. Testes de Integração (`PetTrack.IntegrationTests`)
- **`CustomWebApplicationFactory<Program>`**: Sobe o pipeline HTTP completo em memória substituindo o banco Oracle por um banco **InMemory Database isolado por execução** e populando dados de seed.
- **Validação de Códigos HTTP**:
  - `200 OK`: `GetAll`, `GetById`, `GetPetsComAlertasPendentes`, `/health/live`, `/metrics`.
  - `201 Created`: `Create` com cabeçalho `Location` de recurso criado.
  - `204 NoContent`: `Delete` de recurso existente.
  - `400 BadRequest`: Validações de payload e regras inválidas.
  - `404 NotFound`: Busca e exclusão de IDs inexistentes (`/api/pets/99999`, `/api/tutores/99999`).

### 3. Como Executar os Testes

Para executar toda a suíte de testes unitários e de integração:

```bash
# Executar todos os testes da solução
dotnet test

# Executar apenas testes unitários
dotnet test PetTrack.UnitTests/PetTrack.UnitTests.csproj

# Executar apenas testes de integração
dotnet test PetTrack.IntegrationTests/PetTrack.IntegrationTests.csproj

# Executar testes com relatório detalhado
dotnet test --logger "console;verbosity=detailed"
```

---

## 📡 Endpoints da API REST & Swagger UI

Quando a aplicação estiver em execução, o Swagger UI interativo estará disponível em:
👉 **`http://localhost:5084/swagger`**

| Recurso | Método | Endpoint | Descrição |
|---|---|---|---|
| **Health** | GET | `/health` | Visão completa do status de saúde |
| **Health Live** | GET | `/health/live` | Liveness probe do processo |
| **Health Ready** | GET | `/health/ready` | Readiness probe de banco e serviços |
| **Métricas** | GET | `/metrics` | Exportação de métricas OpenTelemetry / Prometheus |
| **Pets** | GET | `/api/pets` | Lista todos os pets |
| **Pets** | GET | `/api/pets/{id}` | Busca pet por ID |
| **Pets** | GET | `/api/pets/clinica/{idClinica}` | Lista pets por clínica |
| **Pets** | GET | `/api/pets/sexo?sexo=M` | Filtra pets por sexo |
| **Pets** | GET | `/api/pets/buscar?busca=Rex` | Busca por nome ou espécie |
| **Pets** | GET | `/api/pets/alertasPendentes` | Lista pets com alertas não resolvidos |
| **Pets** | POST | `/api/pets` | Cadastra um novo pet |
| **Pets** | PUT | `/api/pets/{id}` | Atualiza dados do pet |
| **Pets** | DELETE | `/api/pets/{id}` | Remove pet do cadastro |
| **Tutores** | GET | `/api/tutores` | Lista todos os tutores |
| **Tutores** | GET | `/api/tutores/{id}` | Busca tutor por ID |
| **Tutores** | GET | `/api/tutores/buscar?busca=Moises` | Busca por nome ou email |
| **Tutores** | POST | `/api/tutores` | Cadastra um novo tutor |
| **Tutores** | PUT | `/api/tutores/{id}` | Atualiza dados do tutor |
| **Tutores** | DELETE | `/api/tutores/{id}` | Remove tutor |
| **Alertas** | GET | `/api/alertas` | Lista alertas gerados |
| **Alertas** | GET | `/api/alertas/tipo?tipo=FEBRE` | Alertas por tipo |
| **Alertas** | GET | `/api/alertas/pendentes/{idPet}` | Alertas pendentes de um pet |
| **Alertas** | POST | `/api/alertas` | Registra novo alerta |
| **Collar** | GET | `/api/collarleituras` | Lista leituras do colar IoT |
| **Collar** | GET | `/api/collarleituras/ultima/{idPet}` | Retorna última telemetria lida |
| **Collar** | POST | `/api/collarleituras` | Registra nova telemetria IoT |
| **Medicamentos** | GET | `/api/medicamentos` | Lista prescrições e medicamentos |
| **Adesões** | GET | `/api/adesoemedicamento` | Histórico de doses administradas |

---

## 🚀 Instruções de Instalação e Execução

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download) ou superior;
- Acesso de rede à base Oracle (VPN FIAP ou rede local do laboratório).

### Passo a Passo

1. **Clonar o Repositório:**
```bash
git clone https://github.com/Challenge-PetTrack/ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET.git
cd ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET/PetTrack
```

2. **Configurar a String de Conexão com Oracle (se necessário):**
No arquivo `PetTrack.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PetTrackContextOracle": "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm563719;Password=fiap;"
  }
}
```

3. **Restaurar os Pacotes NuGet:**
```bash
dotnet restore
```

4. **Compilar a Solução:**
```bash
dotnet build
```

5. **Executar a Suíte de Testes:**
```bash
dotnet test
```

6. **Iniciar a Aplicação:**
```bash
dotnet run --project PetTrack.API/PetTrack.API.csproj
```

---

## 📦 Tecnologias e Pacotes Utilizados

| Pacote / Tecnologia | Versão | Função no Projeto |
|---|---|---|
| **.NET SDK** | 10.0 | Plataforma de desenvolvimento e runtime principal |
| **Oracle.EntityFrameworkCore** | 9.23.60 | Provedor EF Core oficial para banco Oracle |
| **Microsoft.EntityFrameworkCore** | 9.0.14 | ORM para mapeamento objeto-relacional |
| **Serilog.AspNetCore** | 9.0.0 | Structured Logging para Console e arquivos rotativos |
| **Serilog.Sinks.File** | 6.0.0 | Sink de persistência de log com rotação diária |
| **AspNetCore.HealthChecks.UI.Client** | 9.0.0 | Serializador padronizado de Health Checks em JSON |
| **OpenTelemetry.Extensions.Hosting** | 1.11.2 | Framework de observabilidade e tracing distribuído |
| **OpenTelemetry.Exporter.Prometheus.AspNetCore** | 1.11.2-beta.1 | Exportador de métricas no padrão Prometheus (`/metrics`) |
| **xUnit** | 2.9.2 | Framework de execução de testes unitários e de integração |
| **Moq** | 4.20.72 | Framework de Mocking para isolamento de dependências |
| **FluentAssertions** | 6.12.2 | Assertions fluentes e expressivas nos testes AAA |
| **Microsoft.AspNetCore.Mvc.Testing** | 9.0.0 | Factory de testes de integração HTTP in-memory |
| **Microsoft.EntityFrameworkCore.InMemory** | 9.0.0 | Banco de dados em memória para testes de integração |
| **Swashbuckle.AspNetCore** | 7.2.0 | Documentação interativa via OpenAPI / Swagger UI |

---

> Projeto acadêmico desenvolvido para o **Challenge FIAP 2026** — 2º Ano Análise e Desenvolvimento de Sistemas (Turma 2TDSPF).
