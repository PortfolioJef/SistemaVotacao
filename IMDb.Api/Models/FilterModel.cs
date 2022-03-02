using IMDbApi.Models;
using System;

namespace IMDb.Api.Models
{
    public class FilterModel
    {
        //Listagem
        //Opção de filtros por diretor, nome, gênero e/ou atores
        //Opção de trazer registros paginados
        //Retornar a lista ordenada por filmes mais votados e por ordem alfabética
        //Detalhes do filme trazendo todas as informações sobre o filme, inclusive a média dos votos
        public string Name { get; set; }
        public string Director { get; set; }
        public MovieGenre MovieGenre { get; set; }
        public Actor MovieActor { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
