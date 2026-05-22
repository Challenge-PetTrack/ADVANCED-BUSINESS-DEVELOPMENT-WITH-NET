# 🐾 PetTrack API — ASP.NET Core

> Sistema operacional da saúde contínua do pet — API REST em ASP.NET Core 8 conectada ao Oracle DB, desenvolvida para o **Challenge FIAP 2026** em parceria com a **Clyvo Vet**.

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

Esta API em ASP.NET Core compartilha o mesmo banco Oracle já utilizado pela API Java Spring Boot do projeto, garantindo consistência de dados entre ambas as implementações.

---

## 🏗️ Arquitetura — Clean Architecture

```
PetTrack/
├── PetTrack.API/                  → Controllers, Program.cs, Swagger, Exceptions
├── PetTrack.Application/          → DTOs, Interfaces de Services e Repositories, Services
├── PetTrack.Domain/               → Entidades, Enums (regras de negócio puras)
└── PetTrack.Infrastructure/       → DbContext, Configurations EF Core, Repositories
```

**Fluxo de dados:**
```
Request → Controller → Service → Repository → Oracle DB
Response ← Controller ← Service (DTO) ← Entity ← Oracle DB
```

**Regra de dependência:**
```
API → Application → Domain
Infrastructure → Application + Domain
```

---

## 🗄️ Banco de Dados

Banco Oracle compartilhado com a API Java. As tabelas já existem — não são recriadas pelo .NET.

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

### Tutores — `/api/tutores`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/tutores` | Lista todos os tutores | 200 |
| GET | `/api/tutores/{id}` | Busca tutor por ID | 200 / 404 |
| GET | `/api/tutores/buscar?busca=` | Busca por nome ou email | 200 |
| GET | `/api/tutores/nomePet?nomePet=` | Busca por nome do pet | 200 |
| POST | `/api/tutores` | Cria um novo tutor | 201 / 400 |
| PUT | `/api/tutores/{id}` | Atualiza dados do tutor | 200 / 404 |
| DELETE | `/api/tutores/{id}` | Remove um tutor | 204 / 404 |

### Clínicas — `/api/clinicas`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/clinicas` | Lista todas as clínicas | 200 |
| GET | `/api/clinicas/{id}` | Busca clínica por ID | 200 / 404 |
| GET | `/api/clinicas/buscar?busca=` | Busca por nome ou CNPJ | 200 |
| GET | `/api/clinicas/nomePet?nomePet=` | Busca por nome do pet | 200 |
| POST | `/api/clinicas` | Cadastra clínica | 201 / 400 |
| PUT | `/api/clinicas/{id}` | Atualiza clínica | 200 / 404 |
| DELETE | `/api/clinicas/{id}` | Remove clínica | 204 / 404 |

### Pets — `/api/pets`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/pets` | Lista todos os pets | 200 |
| GET | `/api/pets/{id}` | Busca pet por ID | 200 / 404 |
| GET | `/api/pets/clinica/{idClinica}` | Pets por clínica | 200 |
| GET | `/api/pets/sexo?sexo=` | Pets por sexo | 200 |
| GET | `/api/pets/buscar?busca=` | Busca por nome ou espécie | 200 |
| GET | `/api/pets/alertasPendentes` | Pets com alertas pendentes | 200 |
| POST | `/api/pets` | Cadastra um novo pet | 201 / 400 |
| PUT | `/api/pets/{id}` | Atualiza dados do pet | 200 / 404 |
| DELETE | `/api/pets/{id}` | Remove um pet | 204 / 404 |

### Eventos Clínicos — `/api/eventosclinicos`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/eventosclinicos` | Lista todos os eventos | 200 |
| GET | `/api/eventosclinicos/{id}` | Busca evento por ID | 200 / 404 |
| GET | `/api/eventosclinicos/tipo?tipo=` | Eventos por tipo | 200 |
| GET | `/api/eventosclinicos/medicamentos/{idPet}` | Eventos com medicamentos por pet | 200 |
| POST | `/api/eventosclinicos` | Registra evento clínico | 201 / 400 |
| PUT | `/api/eventosclinicos/{id}` | Atualiza evento | 200 / 404 |
| DELETE | `/api/eventosclinicos/{id}` | Remove evento | 204 / 404 |

### Medicamentos — `/api/medicamentos`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/medicamentos` | Lista todos os medicamentos | 200 |
| GET | `/api/medicamentos/{id}` | Busca medicamento por ID | 200 / 404 |
| GET | `/api/medicamentos/buscar?nome=` | Busca por nome | 200 |
| GET | `/api/medicamentos/ativos/{idPet}` | Medicamentos ativos por pet | 200 |
| POST | `/api/medicamentos` | Registra medicamento | 201 / 400 |
| PUT | `/api/medicamentos/{id}` | Atualiza medicamento | 200 / 404 |
| DELETE | `/api/medicamentos/{id}` | Remove medicamento | 204 / 404 |

### Adesões ao Medicamento — `/api/adesoemedicamento`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/adesoemedicamento` | Lista todas as adesões | 200 |
| GET | `/api/adesoemedicamento/{id}` | Busca adesão por ID | 200 / 404 |
| GET | `/api/adesoemedicamento/medicamento/{idMedicamento}` | Adesões por medicamento | 200 |
| GET | `/api/adesoemedicamento/status?status=` | Adesões por status | 200 |
| POST | `/api/adesoemedicamento` | Registra adesão | 201 / 400 |
| PUT | `/api/adesoemedicamento/{id}` | Atualiza adesão | 200 / 404 |
| DELETE | `/api/adesoemedicamento/{id}` | Remove adesão | 204 / 404 |

### Notificações — `/api/notificacoes`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/notificacoes` | Lista todas as notificações | 200 |
| GET | `/api/notificacoes/{id}` | Busca notificação por ID | 200 / 404 |
| GET | `/api/notificacoes/status?status=` | Notificações por status | 200 |
| GET | `/api/notificacoes/tipo?tipo=` | Notificações por tipo | 200 |
| GET | `/api/notificacoes/urgentes/{idTutor}` | Urgentes não lidas por tutor | 200 |
| POST | `/api/notificacoes` | Cria notificação | 201 / 400 |
| PUT | `/api/notificacoes/{id}` | Atualiza notificação | 200 / 404 |
| DELETE | `/api/notificacoes/{id}` | Remove notificação | 204 / 404 |

### Alertas — `/api/alertas`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/alertas` | Lista todos os alertas | 200 |
| GET | `/api/alertas/{id}` | Busca alerta por ID | 200 / 404 |
| GET | `/api/alertas/tipo?tipo=` | Alertas por tipo | 200 |
| GET | `/api/alertas/pendentes/{idPet}` | Alertas pendentes por pet | 200 |
| POST | `/api/alertas` | Cria alerta | 201 / 400 |
| PUT | `/api/alertas/{id}` | Atualiza alerta | 200 / 404 |
| DELETE | `/api/alertas/{id}` | Remove alerta | 204 / 404 |

### BCS Histórico — `/api/bcshistoricos`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/bcshistoricos` | Lista todos os BCS | 200 |
| GET | `/api/bcshistoricos/{id}` | Busca BCS por ID | 200 / 404 |
| GET | `/api/bcshistoricos/historico/{idPet}` | Histórico de BCS por pet | 200 |
| GET | `/api/bcshistoricos/media/{idPet}` | Média de BCS por pet | 200 |
| POST | `/api/bcshistoricos` | Registra BCS | 201 / 400 |
| PUT | `/api/bcshistoricos/{id}` | Atualiza BCS | 200 / 404 |
| DELETE | `/api/bcshistoricos/{id}` | Remove BCS | 204 / 404 |

### Collar Leituras — `/api/collarleituras`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/collarleituras` | Lista todas as leituras | 200 |
| GET | `/api/collarleituras/{id}` | Busca leitura por ID | 200 / 404 |
| GET | `/api/collarleituras/temperatura?idPet=&temperatura=` | Leituras por temperatura | 200 |
| GET | `/api/collarleituras/ultima/{idPet}` | Última leitura por pet | 200 / 404 |
| POST | `/api/collarleituras` | Registra leitura | 201 / 400 |
| PUT | `/api/collarleituras/{id}` | Atualiza leitura | 200 / 404 |
| DELETE | `/api/collarleituras/{id}` | Remove leitura | 204 / 404 |

### Score Histórico — `/api/scoreshistorico`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/scoreshistorico` | Lista todos os scores | 200 |
| GET | `/api/scoreshistorico/{id}` | Busca score por ID | 200 / 404 |
| GET | `/api/scoreshistorico/historico/{idPet}` | Histórico de scores por pet | 200 |
| GET | `/api/scoreshistorico/media/{idPet}` | Média de score por pet | 200 |
| POST | `/api/scoreshistorico` | Registra score | 201 / 400 |
| PUT | `/api/scoreshistorico/{id}` | Atualiza score | 200 / 404 |
| DELETE | `/api/scoreshistorico/{id}` | Remove score | 204 / 404 |

### Protocolos Preventivos — `/api/protocolospreventivos`
| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | `/api/protocolospreventivos` | Lista todos os protocolos | 200 |
| GET | `/api/protocolospreventivos/{id}` | Busca protocolo por ID | 200 / 404 |
| GET | `/api/protocolospreventivos/tipo?tipo=` | Protocolos por tipo | 200 |
| GET | `/api/protocolospreventivos/pendentes/{idPet}` | Pendentes ou atrasados por pet | 200 |
| POST | `/api/protocolospreventivos` | Cria protocolo | 201 / 400 |
| PUT | `/api/protocolospreventivos/{id}` | Atualiza protocolo | 200 / 404 |
| DELETE | `/api/protocolospreventivos/{id}` | Remove protocolo | 204 / 404 |

---

## 🚀 Como Rodar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Acesso à rede da FIAP (VPN ou presencial para o Oracle)
- Git

### 1. Clonar o repositório
```bash
git clone https://github.com/Challenge-PetTrack/ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET.git
cd ADVANCED-BUSINESS-DEVELOPMENT-WITH-NET
```

### 2. Configurar a connection string
Edite `PetTrack.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PetTrackContextOracle": "Data Source=oracle.fiap.com.br:1521/orcl;User ID=rm563719;Password=111206;"
  }
}
```

### 3. Restaurar dependências
```bash
dotnet restore
```

### 4. Aplicar migrations
```bash
dotnet ef migrations add Initial --project PetTrack.Infrastructure --startup-project PetTrack.API
dotnet ef database update --project PetTrack.Infrastructure --startup-project PetTrack.API
```

### 5. Rodar a aplicação
```bash
dotnet run --project PetTrack.API
```

A API estará disponível em:
- Swagger UI: `http://localhost:5084/swagger`

---

## 📦 Pacotes NuGet

| Pacote | Projeto |
|---|---|
| `Oracle.EntityFrameworkCore` | Infrastructure |
| `Microsoft.EntityFrameworkCore` | Infrastructure |
| `Microsoft.EntityFrameworkCore.Tools` | Infrastructure |
| `Swashbuckle.AspNetCore` | API |
| `Microsoft.EntityFrameworkCore.Design` | API |

---

## 📁 Estrutura do Repositório

```
PetTrack/
├── PetTrack.API/
│   ├── Controllers/
│   ├── Exceptions/
│   ├── Swagger/
│   ├── Properties/
│   ├── appsettings.json
│   └── Program.cs
├── PetTrack.Application/
│   ├── DTO/
│   │   ├── Requests/
│   │   └── Responses/
│   └── Interfaces/
│       ├── Repositories/
│       └── Services/
├── PetTrack.Domain/
│   ├── Entities/
│   └── Enum/
├── PetTrack.Infrastructure/
│   └── Persistence/
│       ├── Configurations/
│       ├── Repositories/
│       └── PetTrackContext.cs
└── README.md
```

---

> Projeto desenvolvido para o **Challenge FIAP 2026** — Clyvo Vet × 2TDS Fevereiro
