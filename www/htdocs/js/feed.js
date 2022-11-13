$('#chave').select2({
    tags: true,
    tokenSeparators: [","],
    dropdownParent: $('#filtroModal')
});

var tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
var tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

var pagina = 1;
var listaIds = consultaVagasId();

function consultaVagasId() {
	var lista = new Array();
	$.ajax({
        url: "http://localhost:8888/consulta/vagas/ids",
        type: "GET",
        dataType: "json",
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
                    $(".alert").removeClass("d-none");
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
                    $(".alert").removeClass("d-none");
				break;
				default:
					for(var i = 0; i < retorno.CONTEUDO.length; i++) {
						lista.push(retorno.CONTEUDO[i]);
					}
				break;
			}
        },
		error: function (retorno) {
            $(".alert").addClass("alert-danger");
            if(retorno.status == 404) {
			    $(".alert").html("É obrigatório informar um ID!");
            } else {
			    $(".alert").html("Serviço temporariamente indisponível!");
            }
            $(".alert").removeClass("d-none");
        }
    });
	return lista;
}

setTimeout(() => {
	listarVagas(listaIds,pagina);
}, 100);

function listarVagas(lista, paginaAtual) {
	var idVaga = lista[(paginaAtual-1)];
	if(lista.length > 0) {
		$.ajax({
			url: "http://localhost:8888/consulta/vaga/" + idVaga,
			type: "GET",
			dataType: "json",
			success: function (retorno) {
				switch(retorno.CODIGO) {
					case 1:
						$(".alert").addClass("alert-danger");
						$(".alert").html(retorno.MENSAGEM);
						$(".alert").removeClass("d-none");
					break;
					case 2:
						$(".alert").addClass("alert-danger");
						$(".alert").html(retorno.MENSAGEM);
						$(".alert").removeClass("d-none");
					break;
					default:
						conteudo = '<div class="text-start conteudo_text height-100">';
						conteudo += '<h1>' + retorno.CONTEUDO.EmpresaNome + '</h1>';
						conteudo += '<p>' + retorno.CONTEUDO.Titulo + '</p>';
						conteudo += '<div class="conteudo_text position-absolute bottom-0 start-50 translate-middle-x">';
						conteudo += '<h4><i class="fas fa-arrow-up"></i> Role para ver mais...</h4>';
						conteudo += '</div>';
						conteudo += '</div>';
						conteudo += '<div class="conteudo_vaga text-start">';
						conteudo += '<div class="conteudo_vaga_text">' + retorno.CONTEUDO.EmpresaDescricao + '<br/><br/>' + retorno.CONTEUDO.Descricao + '</div>';
						conteudo += '<hr>';
						conteudo += '<div class="text-center">';
						conteudo += '<div class="d-inline-block px-3 acoes-feed curtir" data-id="' + retorno.CONTEUDO.Id + '" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-custom-class="custom-tooltip" data-bs-title="Curtir Vaga">';
						conteudo += '<i class="fas fa-thumbs-up curtir-icone fa-3x"></i><br/>';
						conteudo += '</div>';
						//conteudo += '<div class="d-inline-block px-3 acoes-feed seguir" data-id="' + retorno.CONTEUDO.EmpresaId + '" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-custom-class="custom-tooltip" data-bs-title="Seguir Empresa">';
						//conteudo += '<i class="fas fa-heart seguir-icone fa-3x"></i><br/>';
						//conteudo += '</div>';
						//conteudo += '<div class="d-inline-block px-3 acoes-feed" data-id="" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-custom-class="custom-tooltip" data-bs-title="Compartilhar Vaga">';
						//conteudo += '<i class="fas fa-share fa-3x"></i><br/>';
						//conteudo += '</div>';
						conteudo += '</div>';
						conteudo += '</div>';
						conteudo += '<script>';
						conteudo += 'tooltipTriggerList = document.querySelectorAll('+"'"+'[data-bs-toggle="tooltip"]'+"'"+');';
						conteudo += 'tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));';
						conteudo += '</script>';
						$(".conteudo").empty();
						$(".conteudo").html(conteudo);
						$.ajax({
							url: "http://localhost:8888/consulta/curtidas/" + Cookies.get("Id") + "/" + idVaga,
							type: "GET",
							dataType: "json",
							success: function (retorno) {
								if(retorno.CODIGO == 0) {
									$(".curtir-icone").css("color", "#450a69");
								} else if(retorno.CODIGO == 1) {
									$(".curtir-icone").css("color", "#b26ddd");
								} else {
									$(".curtir-icone").css("color", "#b26ddd");
								}
							},
							error: function (retorno) {
								$(".alert").addClass("alert-danger");
								if(retorno.status == 404) {
									$(".alert").html("É obrigatório informar um ID!");
								} else {
									$(".alert").html("Serviço temporariamente indisponível!");
								}
								$(".alert").removeClass("d-none");
							}
						});
					break;
				}
			},
			error: function (retorno) {
				$(".alert").addClass("alert-danger");
				if(retorno.status == 404) {
					$(".alert").html("É obrigatório informar um ID!");
				} else {
					$(".alert").html("Serviço temporariamente indisponível!");
				}
				$(".alert").removeClass("d-none");
			}
		});
	}
};

$(document).on("click", ".vagas-retornar", function(event) {
	event.preventDefault();
	if(pagina == 1) {
		return;
	}
	pagina = (pagina - 1);
	listarVagas(listaIds,pagina);
});

$(document).on("click", ".vagas-avancar", function(event) {
	event.preventDefault();
	if(pagina == listaIds.length) {
		return;
	}
	pagina = (pagina + 1);
	listarVagas(listaIds,pagina);
});

$(document).on("click", ".curtir", function(event) {
	event.preventDefault();
	$.ajax({
		url: "http://localhost:8888/vaga/curtir/" + Cookies.get("Id") + "/" + $(this).data("id"),
		type: "GET",
		dataType: "json",
		success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
					$(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
					$(".alert").removeClass("d-none");
				break;
				case 2:
					$(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
					$(".alert").removeClass("d-none");
				break;
				default:
					if (retorno.STATUS == "curtida") {
						$(".curtir-icone").css("color", "#450a69");
					} else {
						$(".curtir-icone").css("color", "#b26ddd");
					}
				break;
			}
		},
		error: function (retorno) {
			$(".alert").addClass("alert-danger");
			if(retorno.status == 404) {
				$(".alert").html("É obrigatório informar um ID!");
			} else {
				$(".alert").html("Serviço temporariamente indisponível!");
			}
			$(".alert").removeClass("d-none");
		}
	});
});

$(document).on("click", ".seguir", function(event) {
	event.preventDefault();
});