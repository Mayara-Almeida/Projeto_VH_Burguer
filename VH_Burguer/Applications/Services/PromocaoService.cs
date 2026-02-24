using VH_Burguer.Applications.Regras;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.PromocaoDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class PromocaoService
    {
        private readonly IPromocaoRepository _repository;

        public PromocaoService(IPromocaoRepository repository)
        {
            _repository = repository;
        }

        public List<LerPromocaoDto> Listar()
        {
            List<Promocao> promocoes = _repository.Listar();

            List<LerPromocaoDto> promocoesDto = promocoes.Select
                (promocao => new LerPromocaoDto
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            }).ToList();

            return promocoesDto; // Promoções que vieram do banco convertidas em DTO
        }

        public LerPromocaoDto ObterPorId(int id)
        {
            Promocao promocao = _repository.ObterPorId(id);

            if (promocao == null)
            {
                throw new DomainException("Promocão não encontrada.");
            }

            LerPromocaoDto promocaoDto = new LerPromocaoDto
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            };

            return promocaoDto;
        }

        private static void ValidarNome(string nome)
        {
            if(string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome é obrigatório.");
            }
        }

        public void Adicionar(CriarPromocaoDto promoDto)
        {
            ValidarNome(promoDto.Nome);
            ValidarDataExpiracaoPromocao.ValidarDataExpiracao(promoDto.DataExpiracao);

            if(_repository.NomeExiste(promoDto.Nome))
            {
                throw new DomainException("Promoção já existente.");
            }

            Promocao promocao = new Promocao
            {
                Nome = promoDto.Nome,
                DataExpiracao = promoDto.DataExpiracao,
                StatusPromocao = promoDto.StatusPromocao
            };

            _repository.Adicionar(promocao);
        }
    }
}
