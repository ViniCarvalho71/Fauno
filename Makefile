# Fauno — orquestracao do projeto inteiro.
#
# Backends (.NET + MySQL) sobem via os docker-compose que ja existem em cada API,
# todos na rede externa compartilhada `fauno-network`. O frontend (NextJS) roda
# no host via bun, apontando para as portas publicadas (8080/8081/8082).
#
# Comando principal:  make up   (sobe tudo de uma vez)
#
# Portas:  Auth 8080 | Agenda 8081 | Register 8082 | Frontend 3000

SHELL := /bin/bash

# Detecta "docker compose" (v2) ou "docker-compose" (v1).
COMPOSE := $(shell docker compose version >/dev/null 2>&1 && echo "docker compose" || echo "docker-compose")

NETWORK := fauno-network

AUTH_DIR     := Fauno.Auth/Fauno.Auth.Api
REGISTER_DIR := Fauno.Register/Fauno.Register
AGENDA_DIR   := Fauno.Agenda/Fauno.Agenda.Api

AUTH     := $(COMPOSE) -p fauno-auth     -f $(AUTH_DIR)/docker-compose.yml
REGISTER := $(COMPOSE) -p fauno-register -f $(REGISTER_DIR)/docker-compose.yml
AGENDA   := $(COMPOSE) -p fauno-agenda   -f $(AGENDA_DIR)/docker-compose.yml

FRONT_DIR := fauno.frontend

.DEFAULT_GOAL := help

.PHONY: help up backend front down stop clean network build ps logs logs-auth logs-register logs-agenda

help: ## Lista os comandos
	@echo "Fauno — comandos disponiveis:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
		| awk 'BEGIN{FS=":.*?## "}{printf "  \033[36m%-14s\033[0m %s\n", $$1, $$2}'

## ----------------------------------------------------------------------------
## Subir tudo
## ----------------------------------------------------------------------------

up: backend ## Sobe backends (docker) e o frontend (bun) — projeto completo
	@echo ""
	@echo "==> Backends no ar:  Auth :8080 | Agenda :8081 | Register :8082"
	@echo "==> Iniciando frontend em http://localhost:3000 (Ctrl+C para parar)"
	@echo ""
	@$(MAKE) front

backend: network ## Sobe apenas os 3 backends (.NET + MySQL) em docker, em background
	@echo "==> Subindo Auth..."
	@$(AUTH) up -d --build
	@echo "==> Subindo Register..."
	@$(REGISTER) up -d --build
	@echo "==> Subindo Agenda..."
	@$(AGENDA) up -d --build

front: ## Roda o frontend NextJS no host (instala deps e sobe o dev server)
	@cd $(FRONT_DIR) && bun install && bun run dev

build: network ## Apenas builda as imagens dos backends (sem subir)
	@$(AUTH) build
	@$(REGISTER) build
	@$(AGENDA) build

network: ## Cria a rede docker compartilhada (idempotente)
	@docker network inspect $(NETWORK) >/dev/null 2>&1 || \
		(echo "==> Criando rede $(NETWORK)" && docker network create $(NETWORK))

## ----------------------------------------------------------------------------
## Parar / limpar
## ----------------------------------------------------------------------------

stop: ## Para os containers sem remover (preserva dados)
	@$(AGENDA) stop
	@$(REGISTER) stop
	@$(AUTH) stop

down: ## Derruba os backends (remove containers, mantem volumes/dados)
	@$(AGENDA) down
	@$(REGISTER) down
	@$(AUTH) down
	-@docker network rm $(NETWORK) 2>/dev/null || true

clean: ## Derruba tudo E apaga os volumes (zera os bancos)
	@$(AGENDA) down -v
	@$(REGISTER) down -v
	@$(AUTH) down -v
	-@docker network rm $(NETWORK) 2>/dev/null || true

## ----------------------------------------------------------------------------
## Status / logs
## ----------------------------------------------------------------------------

ps: ## Mostra o status de todos os containers do Fauno
	@docker ps --filter "name=fauno-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

logs: ## Segue os logs das 3 APIs juntas (Ctrl+C para sair)
	@docker logs -f fauno-auth-api & \
	 docker logs -f fauno-register-api & \
	 docker logs -f fauno-agenda-api & \
	 wait

logs-auth: ## Logs da API de Auth
	@$(AUTH) logs -f

logs-register: ## Logs da API de Register
	@$(REGISTER) logs -f

logs-agenda: ## Logs da API de Agenda
	@$(AGENDA) logs -f
