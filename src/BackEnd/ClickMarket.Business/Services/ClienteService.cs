﻿using AutoMapper;
using ClickMarket.Business.Dtos;
using ClickMarket.Business.Interfaces;
using ClickMarket.Business.Models;
using ClickMarket.Business.Requests;

namespace ClickMarket.Business.Services;

public class ClienteService(
    INotificador notificador,
    IClienteRepository clienteRepository,
    IProdutoRepository produtoRepository,
    IMapper mapper) : BaseService(notificador), IClienteService
{
    public async Task<Cliente> Adicionar(ClienteRequest request)
    {
        var cliente = mapper.Map<ClienteRequest, Cliente>(request);

        if (!request.IsValid())
        {
            Notificar(request.ValidationResult);
            return cliente;
        }

        //Não foi pedido, mas pode ter validação de CPF nesse ponto.

        await clienteRepository.Adicionar(cliente);

        return cliente;
    }

    public async Task<Cliente> Atualizar(Guid id, ClienteRequest request)
    {
        if (id == Guid.Empty)
        {
            Notificar("O ID do cliente não pode ser vazio.");
            return null;
        }

        if (!request.IsValid())
        {
            Notificar(request.ValidationResult);
            return null;
        }

        // Verifica se o cliente existe
        var clienteExistente = await clienteRepository.ObterPorId(id);

        if (clienteExistente == null)
        {
            Notificar("Cliente não encontrado.");
            return null;
        }

        clienteExistente.Nome = request.Nome;
        clienteExistente.Email = request.Email;
        clienteExistente.Telefone = request.Telefone;
        clienteExistente.Ativo = request.Ativo;

        await clienteRepository.Atualizar(clienteExistente);

        return clienteExistente;
    }

    public async Task<ClienteDto> ObterPorId(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notificar("O ID do cliente não pode ser vazio.");
            return null;
        }

        // Verifica se o cliente existe
        var clienteExistente = await clienteRepository.ObterPorId(id);

        if (clienteExistente == null)
        {
            Notificar("Cliente não encontrado.");
            return null;
        }

        // Mapeia o cliente para o DTO
        var clienteDto = mapper.Map<Cliente, ClienteDto>(clienteExistente);

        return clienteDto;
    }

    public async Task<IEnumerable<ClienteDto>> ObterTodos()
    {
        var clientes = await clienteRepository.ObterTodos();

        return mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteDto>>(clientes);
    }

    public async Task Remover(Guid id)
    {
        await clienteRepository.Remover(id);
    }

    public void Dispose()
    {
        clienteRepository?.Dispose();
        GC.SuppressFinalize(this);
    }
}