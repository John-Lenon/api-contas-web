using Application.DTOs.Cobranca;
using AutoMapper;
using Domain.Entities.Cobranca;

namespace Application.Configurations.AutoMapper.Cobranca
{
    public class ContaMapper : Profile
    {
        public ContaMapper()
        {
            CreateMap<ContaDTO, Conta>()
                //.ForMember(destino => destino.DataPagamento, option => option.MapFrom(origem => origem.DataPagamento ?? origem.DataPagamento.GetValueOrDefault()))
                //.ForMember(destino => destino.DataVencimento, option => option.MapFrom(origem => origem.DataVencimento ?? origem.DataVencimento.GetValueOrDefault()))
                //.ForMember(destino => destino.QuantidadeDiasAtraso, option => option.MapFrom(origem => origem.QuantidadeDiasAtraso ?? origem.QuantidadeDiasAtraso.GetValueOrDefault()))
                //.ForMember(destino => destino.Id, option => option.MapFrom(origem => origem.Id ?? origem.Id.GetValueOrDefault()))
                .ReverseMap();

            CreateMap<Conta, ContaGetDTO>().ReverseMap();
        }
    }
}
