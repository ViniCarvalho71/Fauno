Este projeto contém três APIs separadas:

- **Auth**: autenticação e criação de usuário
- **Agenda**: gerenciamento de agenda e agendamentos
- **Register**: cadastro e consulta de informações de registro

## Pré-requisitos

Antes de executar, verifique se você tem instalado:

- Docker
- Docker Compose

## Configuração da rede Docker

O projeto utiliza uma rede Docker chamada `fauno-network` para permitir a comunicação entre os containers.

Crie a rede antes de subir as APIs:

```bash
docker network create fauno-network
```

> Se a rede já existir, o comando pode retornar um aviso informando que ela já foi criada.

## Executando a API Auth

A API de autenticação deve ser executada dentro da pasta:

```bash
Fauno\Fauno.Auth\Fauno.Auth.Api
```

Execute o seguinte comando:

```bash
docker compose up -d --build
```

Após subir, a API ficará disponível em:

- `http://localhost:8080`

### Endpoints da Auth API

- `POST /User/Login` — realiza login e retorna um token JWT.
- `POST /User/CreateUser` — cria um novo usuário.

## Executando a API Agenda

A API de agenda deve ser executada dentro da pasta:

```bash
Fauno\Fauno.Agenda\Fauno.Agenda.Api
```

Execute o seguinte comando:

```bash
docker compose up -d --build
```

Após subir, a API ficará disponível em:

- `http://localhost:8081`

### Endpoints da Agenda API

- `GET /` ou endpoint equivalente de health/raiz, se exposto pela aplicação.
- `POST` de agendamento — cria um novo agendamento.
- `PUT` de confirmação/cancelamento — atualiza o status de um agendamento.
- `GET` de consulta — lista agendamentos ou horários disponíveis.

> Observação: os endpoints exatos podem variar conforme os controllers da aplicação.

## Executando a API Register

A API de registro deve ser executada dentro da pasta:

```bash
Fauno\Fauno.Register\Fauno.Register
```

Execute o seguinte comando:

```bash
docker compose up -d --build
```

Após subir, a API ficará disponível em:

- `http://localhost:8082`

### Endpoints da Register API

- `POST` de criação de registro — cadastra um novo registro.
- `GET` de consulta de registro — busca registros existentes.
- `PUT` de atualização — altera dados de um registro.
- `DELETE` de remoção — remove um registro, se suportado.

> Observação: os endpoints exatos dependem dos controllers presentes no projeto.

## Ordem recomendada de execução

1. Criar a rede `fauno-network`
2. Subir a infraestrutura necessária, se houver banco de dados em compose
3. Executar o `docker compose up -d --build` da API Auth
4. Executar o `docker compose up -d --build` da API Agenda
5. Executar o `docker compose up -d --build` da API Register

## Acesso rápido

- Auth: `http://localhost:8080`
- Agenda: `http://localhost:8081`
- Register: `http://localhost:8082`
