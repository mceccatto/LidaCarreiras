var editorEmpresa;
var cadastrarVaga;
var editorVaga;

ClassicEditor.create(document.querySelector('#descricaoEmpresa'), {
    toolbar: {
        items: [
            'heading',
            'undo',
            'redo',
            'bold',
            'underline',
            'italic',
            'fontFamily',
            'fontSize',
            'fontColor',
            'fontBackgroundColor',
            'horizontalLine',
            'alignment',
            'numberedList',
            'bulletedList',
            'specialCharacters',
            'findAndReplace',
            'insertTable',
            'mediaEmbed',
            'link',
            'code'
        ]
    },
    language: 'pt-br',
    table: {
        contentToolbar: [
            'tableColumn',
            'tableRow',
            'mergeTableCells',
            'tableCellProperties',
            'tableProperties'
        ]
    }
}).then(editor => {
    window.editorEmpresa = editor;
}).catch(error => {
    console.error(error);
});

ClassicEditor.create(document.querySelector('#descricaoVaga'), {
    toolbar: {
        items: [
            'heading',
            'undo',
            'redo',
            'bold',
            'underline',
            'italic',
            'fontFamily',
            'fontSize',
            'fontColor',
            'fontBackgroundColor',
            'horizontalLine',
            'alignment',
            'numberedList',
            'bulletedList',
            'specialCharacters',
            'findAndReplace',
            'insertTable',
            'mediaEmbed',
            'link',
            'code'
        ]
    },
    language: 'pt-br',
    table: {
        contentToolbar: [
            'tableColumn',
            'tableRow',
            'mergeTableCells',
            'tableCellProperties',
            'tableProperties'
        ]
    }
}).then(editor => {
    window.cadastrarVaga = editor;
}).catch(error => {
    console.error(error);
});

$(document).on("click", ".nav-dados-cadastrais", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaEmpresa();
    $("#editarEmpresa").removeClass("d-none");
});

$(document).on("click", ".nav-endereco", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaEndereco();
    $("#editarEndereco").removeClass("d-none");
});

$(document).on("click", ".nav-vagas", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    $.ajax({
        url: "http://localhost:8888/consultas/select/vaga",
        type: "GET",
        dataType: "json",
        success: function (retorno) {
            $("#cadastrarVagas select[name=modalidade]").empty();
            $("#cadastrarVagas select[name=modalidade]").append("<option value='0'>--Selecione</option>");
            for(i = 0; i < retorno.MODALIDADES.length; i++) {
                $("#cadastrarVagas select[name=modalidade]").append("<option value='" + retorno.MODALIDADES[i].Id + "'>" + retorno.MODALIDADES[i].Modalidade + "</option>");
            }
            $("#cadastrarVagas select[name=modelo]").empty();
            $("#cadastrarVagas select[name=modelo]").append("<option value='0'>--Selecione</option>");
            for(i = 0; i < retorno.MODELOS.length; i++) {
                $("#cadastrarVagas select[name=modelo]").append("<option value='" + retorno.MODELOS[i].Id + "'>" + retorno.MODELOS[i].Modelo + "</option>");
            }
            $("#cadastrarVagas select[name=area]").empty();
            $("#cadastrarVagas select[name=area]").append("<option value='0'>--Selecione</option>");
            for(i = 0; i < retorno.AREAS.length; i++) {
                $("#cadastrarVagas select[name=area]").append("<option value='" + retorno.AREAS[i].Id + "'>" + retorno.AREAS[i].Area + "</option>");
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
    buscaVagas();
    $("#cadastrarVagas").removeClass("d-none");
});

$("#editarEmpresa input[name=cnpj]").mask('00.000.000/0000-00', {
    placeholder: "00.000.000/0000-00",
    reverse: true
});

$("#editarEmpresa input[name=telefone]").mask("(00)00000-0000", {
    placeholder: "(00)00000-0000"
});

$("#editarEndereco input[name=cep]").mask("00000-000", {
    placeholder: "00000-000"
});

buscaEmpresa();

function buscaEmpresa() {
    $(".alert").empty();
    $(".alert").removeClass("alert-success alert-danger alert-primary");
    $.ajax({
        url: "http://localhost:8888/empresa/consultar/" + Cookies.get("Id"),
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
                    (retorno.CONTEUDO.Cnpj != "") ? $("#editarEmpresa input[name=cnpj]").val(retorno.CONTEUDO.Cnpj).trigger("input") : null;
                    (retorno.CONTEUDO.RasaoSocial != "") ? $("#editarEmpresa input[name=rasaoSocial]").val(retorno.CONTEUDO.RasaoSocial) : null;
                    (retorno.CONTEUDO.NomeFantasia != "") ? $("#editarEmpresa input[name=nomeFantasia]").val(retorno.CONTEUDO.NomeFantasia) : null;
                    (retorno.CONTEUDO.Telefone != "") ? $("#editarEmpresa input[name=telefone]").val(retorno.CONTEUDO.Telefone).trigger("input") : null;
                    (retorno.CONTEUDO.Ramal != "") ? $("#editarEmpresa input[name=ramal]").val(retorno.CONTEUDO.Ramal) : null;
                    (retorno.CONTEUDO.Descricao != "") ? editorEmpresa.setData(retorno.CONTEUDO.Descricao) : null;
                    (retorno.CONTEUDO.Email != "") ? $("#editarEmpresa input[name=email]").val(retorno.CONTEUDO.Email) : null;
                    if (retorno.CONTEUDO.Logo == "") {
                        $(".previa-logo").css("background-image", "url(../../img/sem-logo.png)");
                        $(".previa-logo").css("background-size", "contain");
                        $(".previa-logo").css("background-position", "center");
                        $(".previa-logo").css("background-repeat", "no-repeat");
                    } else {
                        $(".previa-logo").css("background-image", "url(" + retorno.CONTEUDO.Logo + ")");
                        $(".previa-logo").css("background-size", "contain");
                        $(".previa-logo").css("background-position", "center");
                        $(".previa-logo").css("background-repeat", "no-repeat");
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
    $("#editarEmpresa").removeClass("d-none");
}

function getBase64(file) {
    return new Promise((resolve) => {
        var reader = new FileReader();
        //reader.readAsText(file, "UTF-8");
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
    });
}

$(document).on("change", "#editarEmpresa input[name=logo]", function(event) {
    event.preventDefault();
    var file = $("#editarEmpresa input[name=logo]")[0].files[0];
    getBase64(file).then(data => { LC.logoBase64 = data; });
})

$(document).on("submit", "#editarEmpresa", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#editarEmpresa input[name=rasaoSocial]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo rasão social é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEmpresa input[name=nomeFantasia]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo nome fantasia é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEmpresa input[name=telefone]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo telefone é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEmpresa input[name=senha]").val() != $("#editarEmpresa input[name=repitaSenha]").val()) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("As senhas informadas não são idênticas!");
		$(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    var descricao = editorEmpresa.getData();
	$.ajax({
        url: "http://localhost:8888/empresa/editar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			rasaoSocial:($("#editarEmpresa input[name=rasaoSocial]").val() != "") ? $("#editarEmpresa input[name=rasaoSocial]").val() : null,
			nomeFantasia:($("#editarEmpresa input[name=nomeFantasia]").val() != "") ? $("#editarEmpresa input[name=nomeFantasia]").val() : null,
			telefone:($("#editarEmpresa input[name=telefone]").val() != "") ? $("#editarEmpresa input[name=telefone]").val() : null,
			ramal:($("#editarEmpresa input[name=ramal]").val() != "") ? $("#editarEmpresa input[name=ramal]").val() : null,
            descricao:(descricao != "") ? descricao : null,
            logo:($("#editarEmpresa input[name=logo]")[0].files.length != 0) ? LC.logoBase64 : null,
			senha:($("#editarEmpresa input[name=senha]").val() != "") ? $("#editarEmpresa input[name=senha]").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert").removeClass("alert-success alert-danger alert-primary");
            $(".alert").addClass("alert-primary");
			$(".alert").html("Aguarde...");
			$(".alert").removeClass("d-none");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert").addClass("alert-success");
					$(".alert").html(retorno.MENSAGEM);
				break;
			}
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert").addClass("alert-danger");
			$(".alert").html("Serviço temporariamente indisponível.");
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
    $("#editarEmpresa input[name=senha]").val("");
    $("#editarEmpresa input[name=repitaSenha]").val("");
    window.scrollTo(0, 0);
});

function buscaEndereco() {
    $(".alert").empty();
    $(".alert").removeClass("alert-success alert-danger alert-primary");
    $.ajax({
        url: "http://localhost:8888/empresa/endereco/consultar/" + Cookies.get("Id"),
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
                    (retorno.CONTEUDO.Logradouro != "") ? $("#editarEndereco input[name=logradouro]").val(retorno.CONTEUDO.Logradouro) : null;
                    (retorno.CONTEUDO.Numero != "") ? $("#editarEndereco input[name=numero]").val(retorno.CONTEUDO.Numero) : null;
                    (retorno.CONTEUDO.Complemento != "") ? $("#editarEndereco input[name=complemento]").val(retorno.CONTEUDO.Complemento) : null;
                    (retorno.CONTEUDO.Cep != "") ? $("#editarEndereco input[name=cep]").val(retorno.CONTEUDO.Cep).trigger("input") : null;
                    (retorno.CONTEUDO.Bairro != "") ? $("#editarEndereco input[name=bairro]").val(retorno.CONTEUDO.Bairro) : null;
                    (retorno.CONTEUDO.Cidade != "") ? $("#editarEndereco input[name=cidade]").val(retorno.CONTEUDO.Cidade) : null;
                    (retorno.CONTEUDO.Estado != "") ? $("#editarEndereco select[name=estado]").val(retorno.CONTEUDO.Estado) : null;
				break;
			}
        },
		error: function (retorno) {
            $(".alert").addClass("alert-danger");
            if(retorno.status == 404) {
			    $(".alert").html("Você ainda não possui um endereço cadastrado!");
            } else {
			    $(".alert").html("Serviço temporariamente indisponível!");
            }
            $(".alert").removeClass("d-none");
        }
    });
}

$(document).on("submit", "#editarEndereco", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#editarEndereco input[name=logradouro]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo logradouro é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEndereco select[name=numero] option:selected").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("o campo numero é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEndereco input[name=cep]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo cep é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEndereco input[name=bairro]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo bairro é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEndereco input[name=cidade]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo cidade é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarEndereco select[name=estado] option:selected").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo estado é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/empresa/endereco/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			logradouro:($("#editarEndereco input[name=logradouro]").val() != "") ? $("#editarEndereco input[name=logradouro]").val() : null,
            numero:($("#editarEndereco input[name=numero]").val() != "") ? $("#editarEndereco input[name=numero]").val() : null,
            complemento:($("#editarEndereco input[name=complemento]").val() != "") ? $("#editarEndereco input[name=complemento]").val() : null,
            cep:($("#editarEndereco input[name=cep]").val() != "") ? $("#editarEndereco input[name=cep]").val() : null,
            bairro:($("#editarEndereco input[name=bairro]").val() != "") ? $("#editarEndereco input[name=bairro]").val() : null,
            cidade:($("#editarEndereco input[name=cidade]").val() != "") ? $("#editarEndereco input[name=cidade]").val() : null,
            estado:($("#editarEndereco select[name=estado] option:selected").val() != "") ? $("#editarEndereco select[name=estado] option:selected").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert").removeClass("alert-success alert-danger alert-primary");
            $(".alert").addClass("alert-primary");
			$(".alert").html("Aguarde...");
			$(".alert").removeClass("d-none");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert").addClass("alert-success");
					$(".alert").html(retorno.MENSAGEM);
				break;
			}
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert").addClass("alert-danger");
			$(".alert").html("Serviço temporariamente indisponível.");
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
});

$(document).on("click", ".excluir-endereco", function(event) {
    event.preventDefault();
    $.ajax({
        url: "http://localhost:8888/empresa/endereco/excluir/" + Cookies.get("Id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert").addClass("alert-success");
					$(".alert").html(retorno.MENSAGEM);
                    $("#editarEndereco").trigger("reset");
				break;
			}
            $(".alert").removeClass("d-none");
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

function buscaVagas() {
    $.ajax({
        url: "http://localhost:8888/empresa/vagas/consultar/" + Cookies.get("Id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
			$("#cadastrarVagas #tbodyConteudoVagas").empty();
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").empty();
                    $(".alert").removeClass("alert-success alert-danger alert-primary");
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
                    $(".alert").removeClass("d-none");
				break;
				case 2:
                    $(".alert").empty();
                    $(".alert").removeClass("alert-success alert-danger alert-primary");
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
                    $(".alert").removeClass("d-none");
				break;
				default:
                    var linhas = "";
                    for(i = 0; i < retorno.CONTEUDO.Vagas.length; i++){
                        linhas += '<tr>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Vagas[i].Titulo + '</td>';
                        if(retorno.CONTEUDO.Vagas[i].Status == true) {
                            var status = "Aberta";
                        } else {
                            var status = "Fechada";
                        }
                        linhas += '<td class="align-middle text-center">' + status + '</td>';
                        linhas += '<td class="align-middle text-center">';
                        linhas += '<button class="btn btn-primary btn-sm shadow-none editor-vaga" type="button" data-id="' + retorno.CONTEUDO.Vagas[i].Id + '"><i class="fas fa-pencil-alt"></i></button> ';
                        linhas += '<button class="btn btn-danger btn-sm shadow-none excluir-vaga" type="button" data-id="' + retorno.CONTEUDO.Vagas[i].Id + '"><i class="fas fa-trash-alt"></i></button>';
                        linhas += '</td>';
                        linhas += '</tr>';
                    }
                    $("#cadastrarVagas #tbodyConteudoVagas").html(linhas);
                    $("#cadastrarVagas #tableConteudoVagas").removeClass("d-none");
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

$(document).on("submit", "#cadastrarVagas", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#cadastrarVagas input[name=titulo]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo título é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    var descricao = cadastrarVaga.getData();
    if(descricao == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo descrição é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarVagas select[name=modalidade]").val() == 0) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo modalidade é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarVagas select[name=modelo]").val() == 0) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo modelo é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarVagas select[name=area]").val() == 0) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo área é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/empresa/vaga/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
            titulo:($("#cadastrarVagas input[name=titulo]").val() != "") ? $("#cadastrarVagas input[name=titulo]").val() : null,
            descricao:(descricao != "") ? descricao : null,
            modalidade:($("#cadastrarVagas select[name=modalidade] option:selected").val() != 0) ? $("#cadastrarVagas select[name=modalidade] option:selected").val() : null,
            modelo:($("#cadastrarVagas select[name=modelo] option:selected").val() != 0) ? $("#cadastrarVagas select[name=modelo] option:selected").val() : null,
            area:($("#cadastrarVagas select[name=area] option:selected").val() != 0) ? $("#cadastrarVagas select[name=area] option:selected").val() : null,
            dataLimite:($("#cadastrarVagas input[name=dataLimite]").val() != "") ? $("#cadastrarVagas input[name=dataLimite]").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert").removeClass("alert-success alert-danger alert-primary");
            $(".alert").addClass("alert-primary");
			$(".alert").html("Aguarde...");
			$(".alert").removeClass("d-none");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert").addClass("alert-success");
					$(".alert").html(retorno.MENSAGEM);
                    $("#cadastrarVagas").trigger("reset");
                    cadastrarVaga.setData("");
                    buscaVagas();
				break;
			}
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert").addClass("alert-danger");
			$(".alert").html("Serviço temporariamente indisponível.");
            $(".alert").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
    window.scrollTo(0, 0);
});

$(document).on("click", ".editor-vaga", function(event) {
    event.preventDefault();
    var vagaId = $(this).data("id");
    $.ajax({
        url: "http://localhost:8888/consulta/vaga/" + vagaId,
        type: "GET",
        dataType: "json",
        success: function (retornoVaga) {
            switch(retornoVaga.CODIGO) {
                case 1:
                    $(".alert").addClass("alert-danger");
                    $(".alert").html(retornoVaga.MENSAGEM);
                    $(".alert").removeClass("d-none");
                break;
                case 2:
                    $(".alert").addClass("alert-danger");
                    $(".alert").html(retornoVaga.MENSAGEM);
                    $(".alert").removeClass("d-none");
                break;
                default:
                    $("#modalConteudo").empty();
                    conteudoModal = '<div class="modal-content">';
                    conteudoModal += '<div class="modal-header">';
                    conteudoModal += '<h5 class="modal-title">Editar Vaga</h5>';
                    conteudoModal += '<button type="button" class="btn-close shadow-none" data-bs-dismiss="modal" aria-label="Close"></button>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="modal-body">';
                    conteudoModal += '<div class="alert alert-modal d-none" role="alert"></div>';
                    conteudoModal += '<form id="editarVagas" autocomplete="off">';
                    conteudoModal += '<div class="row g-2">';
                    conteudoModal += '<div class="col-md-12 mb-1">';
                    conteudoModal += '<label class="form-label">Título</label>';
                    conteudoModal += '<input type="text" class="form-control form-control-sm shadow-none" name="editarTitulo" value="' + retornoVaga.CONTEUDO.Titulo + '">';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-12 mb-1">';
                    conteudoModal += '<label class="form-label">Descrição</label>';
                    conteudoModal += '<textarea class="form-control form-control-sm shadow-none" id="editarDescricaoVaga" name="editarDescricao"></textarea>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Modalidade</label>';
                    conteudoModal += '<select class="form-select form-select-sm shadow-none" name="editarModalidade"></select>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Modelo</label>';
                    conteudoModal += '<select class="form-select form-select-sm shadow-none" name="editarModelo"></select>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Área</label>';
                    conteudoModal += '<select class="form-select form-select-sm shadow-none" name="editarArea"></select>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Data Limite</label>';
                    if(retornoVaga.CONTEUDO.DataLimite != "") {
                        conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataLimite" value="' + retornoVaga.CONTEUDO.DataLimite + '">';
                    } else {
                        conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataLimite">';
                    }
                    conteudoModal += '</div>';
                    conteudoModal += '<input type="hidden" name="editarVagaId" value="' + vagaId + '">';
                    conteudoModal += '<div class="d-grid gap-2">';
                    conteudoModal += '<button class="btn btn-primary btn-sm shadow-none" type="submit"><i class="fas fa-save"></i> Salvar Alterações</button>';
                    conteudoModal += '</div>';
                    conteudoModal += '</div>';
                    conteudoModal += '</form>';
                    conteudoModal += '</div>';
                    conteudoModal += '</div>';
                    if(retornoVaga.CONTEUDO.Descricao != null) {
                        conteudoModal += '<script>';
                        conteudoModal += 'ClassicEditor.create(document.querySelector("#editarDescricaoVaga"),{';
                        conteudoModal += 'toolbar:{items:["heading","undo","redo","bold","underline","italic","fontFamily","fontSize","fontColor","fontBackgroundColor","horizontalLine","alignment","numberedList","bulletedList","specialCharacters","findAndReplace","insertTable","mediaEmbed","link","code"]},';
                        conteudoModal += 'language:"pt-br",';
                        conteudoModal += 'table:{contentToolbar:["tableColumn","tableRow","mergeTableCells","tableCellProperties","tableProperties"]}';
                        conteudoModal += '}).then(editor=>{editorVaga=editor;}).catch(error=>{console.error(error);});';
                        conteudoModal += '</script>';
                    } else {
                        conteudoModal += '<script>';
                        conteudoModal += 'ClassicEditor.create(document.querySelector("#editarDescricaoVaga"),{';
                        conteudoModal += 'toolbar:{items:["heading","undo","redo","bold","underline","italic","fontFamily","fontSize","fontColor","fontBackgroundColor","horizontalLine","alignment","numberedList","bulletedList","specialCharacters","findAndReplace","insertTable","mediaEmbed","link","code"]},';
                        conteudoModal += 'language:"pt-br",';
                        conteudoModal += 'table:{contentToolbar:["tableColumn","tableRow","mergeTableCells","tableCellProperties","tableProperties"]}';
                        conteudoModal += '}).then(editor=>{editorVaga=editor;}).catch(error=>{console.error(error);});';
                        conteudoModal += '</script>';
                    }
                    $("#modalConteudo").html(conteudoModal);
                break;
            }

            setTimeout(() => {
                editorVaga.setData(retornoVaga.CONTEUDO.Descricao);
            }, 100);

            $.ajax({
                url: "http://localhost:8888/consultas/select/vaga",
                type: "GET",
                dataType: "json",
                success: function (retorno) {
                    $("#editarVagas select[name=editarModalidade]").empty();
                    $("#editarVagas select[name=editarModalidade]").append("<option value='0'>--Selecione</option>");
                    for(i = 0; i < retorno.MODALIDADES.length; i++) {
                        if(retorno.MODALIDADES[i].Id == retornoVaga.CONTEUDO.ModalidadeId) {
                            $("#editarVagas select[name=editarModalidade]").append("<option value='" + retorno.MODALIDADES[i].Id + "' selected>" + retorno.MODALIDADES[i].Modalidade + "</option>");
                        } else {
                            $("#editarVagas select[name=editarModalidade]").append("<option value='" + retorno.MODALIDADES[i].Id + "'>" + retorno.MODALIDADES[i].Modalidade + "</option>");
                        }
                    }
                    $("#editarVagas select[name=editarModelo]").empty();
                    $("#editarVagas select[name=editarModelo]").append("<option value='0'>--Selecione</option>");
                    for(i = 0; i < retorno.MODELOS.length; i++) {
                        if(retorno.MODELOS[i].Id == retornoVaga.CONTEUDO.ModeloId) {
                            $("#editarVagas select[name=editarModelo]").append("<option value='" + retorno.MODELOS[i].Id + "' selected>" + retorno.MODELOS[i].Modelo + "</option>");
                        } else {
                            $("#editarVagas select[name=editarModelo]").append("<option value='" + retorno.MODELOS[i].Id + "'>" + retorno.MODELOS[i].Modelo + "</option>");
                        }
                    }
                    $("#editarVagas select[name=editarArea]").empty();
                    $("#editarVagas select[name=editarArea]").append("<option value='0'>--Selecione</option>");
                    for(i = 0; i < retorno.AREAS.length; i++) {
                        if(retorno.AREAS[i].Id == retornoVaga.CONTEUDO.AreaId) {
                            $("#editarVagas select[name=editarArea]").append("<option value='" + retorno.AREAS[i].Id + "' selected>" + retorno.AREAS[i].Area + "</option>");
                        } else {
                            $("#editarVagas select[name=editarArea]").append("<option value='" + retorno.AREAS[i].Id + "'>" + retorno.AREAS[i].Area + "</option>");
                        }
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
            $(".modal").modal("show");
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

$(document).on("submit", "#editarVagas", function(event) {
	event.preventDefault();
    $(".alert-modal").empty();
    if($("#editarVagas input[name=editarTitulo]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo título é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    var descricao = editorVaga.getData();
    if(descricao == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo descrição é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarVagas select[name=editarModalidade]").val() == 0) {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo modalidade é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarVagas select[name=editarModelo]").val() == 0) {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo modelo é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarVagas select[name=editarArea]").val() == 0) {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo área é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/empresa/vaga/editar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:$("#editarVagas input[name=editarVagaId]").val(),
            titulo:($("#editarVagas input[name=editarTitulo]").val() != "") ? $("#editarVagas input[name=editarTitulo]").val() : null,
            descricao:(descricao != "") ? descricao : null,
            modalidade:($("#editarVagas select[name=editarModalidade] option:selected").val() != 0) ? $("#editarVagas select[name=editarModalidade] option:selected").val() : null,
            modelo:($("#editarVagas select[name=editarModelo] option:selected").val() != 0) ? $("#editarVagas select[name=editarModelo] option:selected").val() : null,
            area:($("#editarVagas select[name=editarArea] option:selected").val() != 0) ? $("#editarVagas select[name=editarArea] option:selected").val() : null,
            dataLimite:($("#editarVagas input[name=editarDataLimite]").val() != "") ? $("#editarVagas input[name=editarDataLimite]").val() : null,
            status:true
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert-modal").removeClass("alert-success alert-danger alert-primary");
            $(".alert-modal").addClass("alert-primary");
			$(".alert-modal").html("Aguarde...");
			$(".alert-modal").removeClass("d-none");
		},
        success: function (retorno) {
            console.log(retorno);
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert-modal").addClass("alert-danger");
					$(".alert-modal").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert-modal").addClass("alert-danger");
					$(".alert-modal").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert-modal").addClass("alert-success");
					$(".alert-modal").html(retorno.MENSAGEM);
                    buscaVagas();
				break;
			}
            $(".alert-modal").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert-modal").addClass("alert-danger");
			$(".alert-modal").html("Serviço temporariamente indisponível.");
            $(".alert-modal").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
    $(".modal").scrollTop(0);
});

$(document).on("click", ".excluir-vaga", function(event) {
    event.preventDefault();
    var linha = $(this);
    $.ajax({
        url: "http://localhost:8888/empresa/vaga/excluir/" + $(this).data("id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert").addClass("alert-danger");
					$(".alert").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert").addClass("alert-success");
					$(".alert").html(retorno.MENSAGEM);
                    $(linha).closest("tr").remove();
				break;
			}
            $(".alert").removeClass("d-none");
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