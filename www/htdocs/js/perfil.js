var editorPerfil;
var cadastrarExperiencias;
var editorExperiencias;

ClassicEditor.create(document.querySelector('#sobre'), {
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
    window.editorPerfil = editor;
}).catch(error => {
    console.error(error);
});

ClassicEditor.create(document.querySelector('#descricao'), {
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
    window.cadastrarExperiencias = editor;
}).catch(error => {
    console.error(error);
});

$(document).on("click", ".nav-dados-pessoais", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaPerfil();
    $("#editarPerfil").removeClass("d-none");
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

$(document).on("click", ".nav-conhecimentos", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaConhecimentos();
    $("#editarConhecimentos").removeClass("d-none");
});

$(document).on("click", ".nav-formacoes", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaFormacoes();
    $("#cadastrarFormacoes").removeClass("d-none");
});

$(document).on("click", ".nav-experiencias", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaExperiencias();
    $("#cadastrarExperiencias").removeClass("d-none");
});

$(document).on("click", ".nav-certificados", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $(".nav-link").removeClass("active");
    $(this).addClass("active");
    $("form").addClass("d-none");
    buscaCertificados();
    $("#cadastrarCertificados").removeClass("d-none");
});

$("#editarPerfil input[name=telefone]").mask("(00)00000-0000", {
    placeholder: "(00)00000-0000"
});

$("#editarEndereco input[name=cep]").mask("00000-000", {
    placeholder: "00000-000"
});

$("#editarConhecimentos select[name=conhecimentos]").select2({
    tags: true
});

buscaPerfil();

function buscaPerfil() {
    $(".alert").empty();
    $(".alert").removeClass("alert-success alert-danger alert-primary");
    $.ajax({
        url: "http://localhost:8888/candidato/consultar/" + Cookies.get("Id"),
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
                    (retorno.CONTEUDO.Nome != "") ? $("#editarPerfil input[name=nome]").val(retorno.CONTEUDO.Nome) : null;
                    (retorno.CONTEUDO.NomeSocial != "") ? $("#editarPerfil input[name=nomeSocial]").val(retorno.CONTEUDO.NomeSocial) : null;
                    (retorno.CONTEUDO.Genero != "") ? $("#editarPerfil select[name=genero]").val(retorno.CONTEUDO.Genero) : null;
                    (retorno.CONTEUDO.DataNascimento != "") ? $("#editarPerfil input[name=dataNascimento]").val(retorno.CONTEUDO.DataNascimento) : null;
                    (retorno.CONTEUDO.Telefone != "") ? $("#editarPerfil input[name=telefone]").val(retorno.CONTEUDO.Telefone).trigger("input") : null;
                    (retorno.CONTEUDO.Email != "") ? $("#editarPerfil input[name=email]").val(retorno.CONTEUDO.Email) : null;
                    (retorno.CONTEUDO.Sobre != null) ? editorPerfil.setData(retorno.CONTEUDO.Sobre) : null;
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
    $("#editarPerfil").removeClass("d-none");
}

$(document).on("submit", "#editarPerfil", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#editarPerfil input[name=nome]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo nome completo é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarPerfil select[name=genero] option:selected").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("o campo gênero é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarPerfil input[name=dataNascimento]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo data de nascimento é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarPerfil input[name=telefone]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo telefone é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarPerfil input[name=senha]").val() != $("#editarPerfil input[name=repitaSenha]").val()) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("As senhas informadas não são idênticas!");
		$(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    var sobre = editorPerfil.getData();
	$.ajax({
        url: "http://localhost:8888/candidato/editar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			nome:($("#editarPerfil input[name=nome]").val() != "") ? $("#editarPerfil input[name=nome]").val() : null,
			nomeSocial:($("#editarPerfil input[name=nomeSocial]").val() != "") ? $("#editarPerfil input[name=nomeSocial]").val() : null,
			genero:($("#editarPerfil select[name=genero] option:selected").val() != "") ? $("#editarPerfil select[name=genero] option:selected").val() : null,
			dataNascimento:($("#editarPerfil input[name=dataNascimento]").val() != "") ? $("#editarPerfil input[name=dataNascimento]").val() : null,
			telefone:($("#editarPerfil input[name=telefone]").val() != "") ? $("#editarPerfil input[name=telefone]").val() : null,
            sobre:(sobre != "") ? sobre : null,
			senha:($("#editarPerfil input[name=senha]").val() != "") ? $("#editarPerfil input[name=senha]").val() : null
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
    $("#editarPerfil input[name=senha]").val("");
    $("#editarPerfil input[name=repitaSenha]").val("");
});

function buscaEndereco() {
    $(".alert").empty();
    $(".alert").removeClass("alert-success alert-danger alert-primary");
    $.ajax({
        url: "http://localhost:8888/candidato/endereco/consultar/" + Cookies.get("Id"),
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
        url: "http://localhost:8888/candidato/endereco/registrar",
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
        url: "http://localhost:8888/candidato/endereco/excluir/" + Cookies.get("Id"),
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

function buscaConhecimentos() {
    $(".alert").empty();
    $(".alert").removeClass("alert-success alert-danger alert-primary");
    $.ajax({
        url: "http://localhost:8888/candidato/conhecimento/consultar/" + Cookies.get("Id"),
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
                    if(retorno.CONTEUDO.Conhecimentos != "") {
                        $("#editarConhecimentos select[name=conhecimentos]").empty();
                        for(i = 0; i < retorno.CONTEUDO.Conhecimentos.length; i++) {
                            $("#editarConhecimentos select[name=conhecimentos]").append("<option value='" + retorno.CONTEUDO.Conhecimentos[i]["Conhecimento"] + "' selected>" + retorno.CONTEUDO.Conhecimentos[i]["Conhecimento"] + "</option>").trigger("change");
                        }
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
}

$(document).on("submit", "#editarConhecimentos", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#editarConhecimentos select[name=conhecimentos]").select2('data').length == 0) {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo conhecimentos é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    conhecimentos = new Array();
    for(i = 0; i < $("#editarConhecimentos select[name=conhecimentos]").select2('data').length; i++) {
        conhecimentos[i] = $("#editarConhecimentos select[name=conhecimentos]").select2('data')[i].id.toUpperCase();
    }
	$.ajax({
        url: "http://localhost:8888/candidato/conhecimento/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			conhecimento:conhecimentos
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

$(document).on("click", ".excluir-conhecimentos", function(event) {
    event.preventDefault();
    $.ajax({
        url: "http://localhost:8888/candidato/conhecimento/excluir/" + Cookies.get("Id"),
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
                    $("#editarConhecimentos select[name=conhecimentos]").val("").trigger("change");
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

function buscaFormacoes() {
    $.ajax({
        url: "http://localhost:8888/candidato/formacao/consultar/" + Cookies.get("Id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
            $("#cadastrarFormacoes #tbodyConteudoFormacoes").empty();
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
                    for(i = 0; i < retorno.CONTEUDO.Formacoes.length; i++){
                        linhas += '<tr>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Formacoes[i].Instituicao + '</td>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Formacoes[i].Curso + '</td>';
                        linhas += '<td class="align-middle text-center">' + retorno.CONTEUDO.Formacoes[i].DataInicio + '</td>';
                        linhas += '<td class="align-middle text-center">' + retorno.CONTEUDO.Formacoes[i].DataConclusao + '</td>';
                        linhas += '<td class="align-middle text-center">';
                        linhas += '<button class="btn btn-primary btn-sm shadow-none editor-formacao" type="button" data-id="' + retorno.CONTEUDO.Formacoes[i].Instituicao + '|' + retorno.CONTEUDO.Formacoes[i].Curso + '|' + retorno.CONTEUDO.Formacoes[i].DataInicio + '|' + retorno.CONTEUDO.Formacoes[i].DataConclusao + '|' + retorno.CONTEUDO.Formacoes[i].DataCadastro + '"><i class="fas fa-pencil-alt"></i></button> ';
                        linhas += '<button class="btn btn-danger btn-sm shadow-none excluir-formacao" type="button" data-id="' + retorno.CONTEUDO.Formacoes[i].DataCadastro + '"><i class="fas fa-trash-alt"></i></button>';
                        linhas += '</td>';
                        linhas += '</tr>';
                    }
                    $("#cadastrarFormacoes #tbodyConteudoFormacoes").html(linhas);
                    $("#cadastrarFormacoes #tableConteudoFormacoes").removeClass("d-none");
				break;
			}
        },
		error: function (retorno) {
            $(".alert").empty();
            $(".alert").removeClass("alert-success alert-danger alert-primary");
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

$(document).on("submit", "#cadastrarFormacoes", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#cadastrarFormacoes input[name=instituicao]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo instituição é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarFormacoes input[name=curso]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo curso é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarFormacoes input[name=dataInicio]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo data início é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/candidato/formacao/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			instituicao:($("#cadastrarFormacoes input[name=instituicao]").val() != "") ? $("#cadastrarFormacoes input[name=instituicao]").val() : null,
			curso:($("#cadastrarFormacoes input[name=curso]").val() != "") ? $("#cadastrarFormacoes input[name=curso]").val() : null,
			dataInicio:($("#cadastrarFormacoes input[name=dataInicio]").val() != "") ? $("#cadastrarFormacoes input[name=dataInicio]").val() : null,
			dataConclusao:($("#cadastrarFormacoes input[name=dataConclusao]").val() != "") ? $("#cadastrarFormacoes input[name=dataConclusao]").val() : null
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
                    $("#cadastrarFormacoes").trigger("reset");
                    buscaFormacoes();
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

$(document).on("click", ".editor-formacao", function(event) {
    event.preventDefault();
    var dados = $(this).data("id").split("|");
    $("#modalConteudo").empty();
    conteudoModal = '<div class="modal-content">';
    conteudoModal += '<div class="modal-header">';
    conteudoModal += '<h5 class="modal-title">Editar Formação</h5>';
    conteudoModal += '<button type="button" class="btn-close shadow-none" data-bs-dismiss="modal" aria-label="Close"></button>';
    conteudoModal += '</div>';
    conteudoModal += '<div class="modal-body">';
    conteudoModal += '<div class="alert alert-modal d-none" role="alert"></div>';
    conteudoModal += '<form id="editarFormacoes" autocomplete="off">';
    conteudoModal += '<div class="row g-2">';
    conteudoModal += '<div class="col-md-12 mb-1">';
    conteudoModal += '<label class="form-label">Instituição</label>';
    conteudoModal += '<input type="text" class="form-control form-control-sm shadow-none" name="editarInstituicao" value="' + dados[0] + '">';
    conteudoModal += '</div>';
    conteudoModal += '<div class="col-md-12 mb-1">';
    conteudoModal += '<label class="form-label">Curso</label>';
    conteudoModal += '<input type="text" class="form-control form-control-sm shadow-none" name="editarCurso" value="' + dados[1] + '">';
    conteudoModal += '</div>';
    conteudoModal += '<div class="col-md-6 mb-1">';
    conteudoModal += '<label class="form-label">Data Início</label>';
    conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataInicio" value="' + dados[2].split("/")[2] + "-" + dados[2].split("/")[1] + "-" + dados[2].split("/")[0] + '">';
    conteudoModal += '</div>';
    conteudoModal += '<div class="col-md-6 mb-1">';
    conteudoModal += '<label class="form-label">Data Conclusão</label>';
    conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataConclusao" value="' + dados[3].split("/")[2] + "-" + dados[3].split("/")[1] + "-" + dados[3].split("/")[0] + '">';
    conteudoModal += '</div>';
    conteudoModal += '<input type="hidden" name="editarDataCadastro" value="' + dados[4] + '">';
    conteudoModal += '<div class="d-grid gap-2">';
    conteudoModal += '<button class="btn btn-primary btn-sm shadow-none" type="submit"><i class="fas fa-save"></i> Salvar Alterações</button>';
    conteudoModal += '</div>';
    conteudoModal += '</div>';
    conteudoModal += '</form>';
    conteudoModal += '</div>';
    conteudoModal += '</div>';
    $("#modalConteudo").html(conteudoModal);
    $(".modal").modal("show");
});

$(document).on("submit", "#editarFormacoes", function(event) {
	event.preventDefault();
    $(".alert-modal").empty();
    if($("#editarFormacoes input[name=editarInstituicao]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo instituição é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarFormacoes input[name=editarCurso]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo curso é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarFormacoes input[name=editarDataInicio]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo data início é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/candidato/formacao/editar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			instituicao:($("#editarFormacoes input[name=editarInstituicao]").val() != "") ? $("#editarFormacoes input[name=editarInstituicao]").val() : null,
			curso:($("#editarFormacoes input[name=editarCurso]").val() != "") ? $("#editarFormacoes input[name=editarCurso]").val() : null,
			dataInicio:($("#editarFormacoes input[name=editarDataInicio]").val() != "") ? $("#editarFormacoes input[name=editarDataInicio]").val() : null,
			dataConclusao:($("#editarFormacoes input[name=editarDataConclusao]").val() != "") ? $("#editarFormacoes input[name=editarDataConclusao]").val() : null,
			dataCadastro:($("#editarFormacoes input[name=editarDataCadastro]").val() != "") ? $("#editarFormacoes input[name=editarDataCadastro]").val() : null
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
                    buscaFormacoes();
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
});

$(document).on("click", ".excluir-formacao", function(event) {
    event.preventDefault();
    var linha = $(this);
    $.ajax({
        url: "http://localhost:8888/candidato/formacao/excluir/" + Cookies.get("Id") + "/" + $(this).data("id"),
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

function buscaExperiencias() {
    $.ajax({
        url: "http://localhost:8888/candidato/experiencia/consultar/" + Cookies.get("Id") + "/0",
        type: "GET",
        dataType: "json",
        success: function (retorno) {
            $("#cadastrarExperiencias #tbodyConteudoExperiencias").empty();
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
                    for(i = 0; i < retorno.CONTEUDO.Experiencias.length; i++){
                        linhas += '<tr>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Experiencias[i].Empresa + '</td>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Experiencias[i].Funcao + '</td>';
                        linhas += '<td class="align-middle text-center">' + retorno.CONTEUDO.Experiencias[i].DataContratacao + '</td>';
                        linhas += '<td class="align-middle text-center">' + retorno.CONTEUDO.Experiencias[i].DataDesligamento + '</td>';
                        linhas += '<td class="align-middle text-center">';
                        linhas += '<button class="btn btn-primary btn-sm shadow-none editor-experiencia" type="button" data-id="' + retorno.CONTEUDO.Experiencias[i].DataCadastro + '"><i class="fas fa-pencil-alt"></i></button> ';
                        linhas += '<button class="btn btn-danger btn-sm shadow-none excluir-experiencia" type="button" data-id="' + retorno.CONTEUDO.Experiencias[i].DataCadastro + '"><i class="fas fa-trash-alt"></i></button>';
                        linhas += '</td>';
                        linhas += '</tr>';
                    }
                    $("#cadastrarExperiencias #tbodyConteudoExperiencias").html(linhas);
                    $("#cadastrarExperiencias #tableConteudoExperiencias").removeClass("d-none");
				break;
			}
        },
		error: function (retorno) {
            $(".alert").empty();
            $(".alert").removeClass("alert-success alert-danger alert-primary");
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

$(document).on("submit", "#cadastrarExperiencias", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#cadastrarExperiencias input[name=empresa]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo empresa é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarExperiencias input[name=funcao]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo função é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarExperiencias input[name=dataContratacao]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo contratação é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarExperiencias textarea[name=descricao]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo descrição é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    var descricao = cadastrarExperiencias.getData();
	$.ajax({
        url: "http://localhost:8888/candidato/experiencia/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			empresa:($("#cadastrarExperiencias input[name=empresa]").val() != "") ? $("#cadastrarExperiencias input[name=empresa]").val() : null,
			funcao:($("#cadastrarExperiencias input[name=funcao]").val() != "") ? $("#cadastrarExperiencias input[name=funcao]").val() : null,
			dataContratacao:($("#cadastrarExperiencias input[name=dataContratacao]").val() != "") ? $("#cadastrarExperiencias input[name=dataContratacao]").val() : null,
			dataDesligamento:($("#cadastrarExperiencias input[name=dataDesligamento]").val() != "") ? $("#cadastrarExperiencias input[name=dataDesligamento]").val() : null,
            descricao:(descricao != "") ? descricao : null
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
                    $("#cadastrarExperiencias").trigger("reset");
                    cadastrarExperiencias.setData("");
                    buscaExperiencias();
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

$(document).on("click", ".editor-experiencia", function(event) {
    event.preventDefault();



    $.ajax({
        url: "http://localhost:8888/candidato/experiencia/consultar/" + Cookies.get("Id") + "/" + $(this).data("id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
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
                    $("#modalConteudo").empty();
                    conteudoModal = '<div class="modal-content">';
                    conteudoModal += '<div class="modal-header">';
                    conteudoModal += '<h5 class="modal-title">Editar Experiência</h5>';
                    conteudoModal += '<button type="button" class="btn-close shadow-none" data-bs-dismiss="modal" aria-label="Close"></button>';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="modal-body">';
                    conteudoModal += '<div class="alert alert-modal d-none" role="alert"></div>';
                    conteudoModal += '<form id="editarExperiencias" autocomplete="off">';
                    conteudoModal += '<div class="row g-2">';
                    conteudoModal += '<div class="col-md-12 mb-1">';
                    conteudoModal += '<label class="form-label">Empresa</label>';
                    conteudoModal += '<input type="text" class="form-control form-control-sm shadow-none" name="editarEmpresa" value="' + retorno.CONTEUDO.Experiencias[0].Empresa + '">';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-12 mb-1">';
                    conteudoModal += '<label class="form-label">Função</label>';
                    conteudoModal += '<input type="text" class="form-control form-control-sm shadow-none" name="editarFuncao" value="' + retorno.CONTEUDO.Experiencias[0].Funcao + '">';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Contratação</label>';
                    conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataContratacao" value="' + retorno.CONTEUDO.Experiencias[0].DataContratacao.split("/")[2] + "-" + retorno.CONTEUDO.Experiencias[0].DataContratacao.split("/")[1] + "-" + retorno.CONTEUDO.Experiencias[0].DataContratacao.split("/")[0] + '">';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-6 mb-1">';
                    conteudoModal += '<label class="form-label">Desligamento</label>';
                    conteudoModal += '<input type="date" class="form-control form-control-sm shadow-none" name="editarDataDesligamento" value="' + retorno.CONTEUDO.Experiencias[0].DataDesligamento.split("/")[2] + "-" + retorno.CONTEUDO.Experiencias[0].DataDesligamento.split("/")[1] + "-" + retorno.CONTEUDO.Experiencias[0].DataDesligamento.split("/")[0] + '">';
                    conteudoModal += '</div>';
                    conteudoModal += '<div class="col-md-12 mb-1">';
                    conteudoModal += '<label class="form-label">Descrição da Atividades</label>';
                    conteudoModal += '<textarea class="form-control form-control-sm shadow-none" id="editarDescricao" name="editarDescricao"></textarea>';
                    conteudoModal += '</div>';
                    conteudoModal += '<input type="hidden" name="editarDataCadastro" value="' + retorno.CONTEUDO.Experiencias[0].DataCadastro + '">';
                    conteudoModal += '<div class="d-grid gap-2">';
                    conteudoModal += '<button class="btn btn-primary btn-sm shadow-none" type="submit"><i class="fas fa-save"></i> Salvar Alterações</button>';
                    conteudoModal += '</div>';
                    conteudoModal += '</div>';
                    conteudoModal += '</form>';
                    conteudoModal += '</div>';
                    conteudoModal += '</div>';
                    if(retorno.CONTEUDO.Experiencias[0].Descricao != null) {
                        conteudoModal += '<script>';
                        conteudoModal += 'ClassicEditor.create(document.querySelector("#editarDescricao"),{';
                        conteudoModal += 'toolbar:{items:["heading","undo","redo","bold","underline","italic","fontFamily","fontSize","fontColor","fontBackgroundColor","horizontalLine","alignment","numberedList","bulletedList","specialCharacters","findAndReplace","insertTable","mediaEmbed","link","code"]},';
                        conteudoModal += 'language:"pt-br",';
                        conteudoModal += 'table:{contentToolbar:["tableColumn","tableRow","mergeTableCells","tableCellProperties","tableProperties"]}';
                        conteudoModal += '}).then(editor=>{editorExperiencias=editor;}).catch(error=>{console.error(error);});';
                        conteudoModal += '</script>';
                    } else {
                        conteudoModal += '<script>';
                        conteudoModal += 'ClassicEditor.create(document.querySelector("#editarDescricao"),{';
                        conteudoModal += 'toolbar:{items:["heading","undo","redo","bold","underline","italic","fontFamily","fontSize","fontColor","fontBackgroundColor","horizontalLine","alignment","numberedList","bulletedList","specialCharacters","findAndReplace","insertTable","mediaEmbed","link","code"]},';
                        conteudoModal += 'language:"pt-br",';
                        conteudoModal += 'table:{contentToolbar:["tableColumn","tableRow","mergeTableCells","tableCellProperties","tableProperties"]}';
                        conteudoModal += '}).then(editor=>{editorExperiencias=editor;}).catch(error=>{console.error(error);});';
                        conteudoModal += '</script>';
                    }
                    $("#modalConteudo").html(conteudoModal);
                    setTimeout(() => {
                        editorExperiencias.setData(retorno.CONTEUDO.Experiencias[0].Descricao);
                    }, 100);
                    $(".modal").modal("show");
				break;
			}
        },
		error: function (retorno) {
            $(".alert").empty();
            $(".alert").removeClass("alert-success alert-danger alert-primary");
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

$(document).on("submit", "#editarExperiencias", function(event) {
	event.preventDefault();
    $(".alert-modal").empty();
    if($("#editarExperiencias input[name=editarEmpresa]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo empresa é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarExperiencias input[name=editarFuncao]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo função é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarExperiencias input[name=editarDataContratacao]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo contratação é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#editarExperiencias textarea[name=editarDescricao]").val() == "") {
        $(".alert-modal").addClass("alert-danger");
        $(".alert-modal").html("O campo descrição é obrigatório!");
        $(".alert-modal").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/candidato/experiencia/editar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			empresa:($("#editarExperiencias input[name=editarEmpresa]").val() != "") ? $("#editarExperiencias input[name=editarEmpresa]").val() : null,
			funcao:($("#editarExperiencias input[name=editarFuncao]").val() != "") ? $("#editarExperiencias input[name=editarFuncao]").val() : null,
			dataContratacao:($("#editarExperiencias input[name=editarDataContratacao]").val() != "") ? $("#editarExperiencias input[name=editarDataContratacao]").val() : null,
			dataDesligamento:($("#editarExperiencias input[name=editarDataDesligamento]").val() != "") ? $("#editarExperiencias input[name=editarDataDesligamento]").val() : null,
            descricao:($("#editarExperiencias textarea[name=editarDescricao]").val() != "") ? $("#editarExperiencias textarea[name=editarDescricao]").val() : null,
			dataCadastro:($("#editarExperiencias input[name=editarDataCadastro]").val() != "") ? $("#editarExperiencias input[name=editarDataCadastro]").val() : null
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
                    buscaExperiencias();
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

$(document).on("click", ".excluir-experiencia", function(event) {
    event.preventDefault();
    var linha = $(this);
    $.ajax({
        url: "http://localhost:8888/candidato/experiencia/excluir/" + Cookies.get("Id") + "/" + $(this).data("id"),
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

function getBase64(file) {
    return new Promise((resolve) => {
        var reader = new FileReader();
        //reader.readAsText(file, "UTF-8");
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
    });
}

$(document).on("change", "#cadastrarCertificados input[name=anexo]", function(event) {
    event.preventDefault();
    var file = $("#cadastrarCertificados input[name=anexo]")[0].files[0];
    getBase64(file).then(data => {LC.anexoBase64 = data;});
})

function buscaCertificados() {
    $.ajax({
        url: "http://localhost:8888/candidato/certificado/consultar/" + Cookies.get("Id"),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
            $("#cadastrarCertificados #tbodyConteudoCertificados").empty();
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
                    for(i = 0; i < retorno.CONTEUDO.Certificados.length; i++){
                        linhas += '<tr>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Certificados[i].Instituicao + '</td>';
                        linhas += '<td class="align-middle">' + retorno.CONTEUDO.Certificados[i].Titulo + '</td>';
                        if(retorno.CONTEUDO.Certificados[i].Anexo == "") {
                            linhas += '<td class="align-middle text-center">-</td>';
                        } else {
                            linhas += '<td class="align-middle text-center"><i class="fas fa-file-pdf certificado-anexo" data-id="' + retorno.CONTEUDO.Certificados[i].Anexo + '" style="font-size:25px;"></i></td>';
                        }
                        if(retorno.CONTEUDO.Certificados[i].Url == "") {
                            linhas += '<td class="align-middle text-center">-</td>';
                        } else {
                            linhas += '<td class="align-middle text-center"><a href="' + retorno.CONTEUDO.Certificados[i].Url + '" class="link-certificado" target="_blank"><i class="fas fa-link" style="font-size:25px;"></i></a></td>';
                        }
                        linhas += '<td class="align-middle text-center">';
                        linhas += '<button class="btn btn-danger btn-sm shadow-none excluir-certificado" type="button" data-id="' + retorno.CONTEUDO.Certificados[i].DataCadastro + '"><i class="fas fa-trash-alt"></i></button>';
                        linhas += '</td>';
                        linhas += '</tr>';
                    }
                    $("#cadastrarCertificados #tbodyConteudoCertificados").html(linhas);
                    $("#cadastrarCertificados #tableConteudoCertificados").removeClass("d-none");
				break;
			}
        },
		error: function (retorno) {
            $(".alert").empty();
            $(".alert").removeClass("alert-success alert-danger alert-primary");
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

$(document).on("submit", "#cadastrarCertificados", function(event) {
	event.preventDefault();
    $(".alert").empty();
    if($("#cadastrarCertificados input[name=instituicao]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo instituicao é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarCertificados input[name=titulo]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("O campo título é obrigatório!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarCertificados input[name=anexo]")[0].files.length == 0 && $("#cadastrarCertificados input[name=url]").val() == "") {
        $(".alert").addClass("alert-danger");
        $(".alert").html("Você deve inserir um anexo ou informar uma url!");
        $(".alert").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#cadastrarCertificados input[name=anexo]")[0].files.length != 0) {
        if($("#cadastrarCertificados input[name=anexo]")[0].files[0].type != "application/pdf") {
            $(".alert").addClass("alert-danger");
            $(".alert").html("Apenas arquivo PDF é permitido!");
            $(".alert").removeClass("d-none");
            $("button").blur();
            return;
        }
    }
	$.ajax({
        url: "http://localhost:8888/candidato/certificado/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            id:Cookies.get("Id"),
			instituicao:($("#cadastrarCertificados input[name=instituicao]").val() != "") ? $("#cadastrarCertificados input[name=instituicao]").val() : null,
			titulo:($("#cadastrarCertificados input[name=titulo]").val() != "") ? $("#cadastrarCertificados input[name=titulo]").val() : null,
            anexo:($("#cadastrarCertificados input[name=anexo]")[0].files.length != 0) ? LC.anexoBase64 : null,
            //anexo:($("#cadastrarCertificados input[name=anexo]")[0].files.length != 0) ? LC.anexoBase64.replace(/^data:(.*,)?/, '') : null,
            url:($("#cadastrarCertificados input[name=url]").val() != "") ? $("#cadastrarCertificados input[name=url]").val() : null
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
                    $("#cadastrarCertificados").trigger("reset");
                    buscaCertificados();
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

$(document).on("click", ".certificado-anexo", function(event) {
    event.preventDefault();
    console.log($(this).data("id"));
    conteudoModal = '<div class="modal-content">';
    conteudoModal += '<div class="modal-header">';
    conteudoModal += '<h5 class="modal-title">Certificado</h5>';
    conteudoModal += '<button type="button" class="btn-close shadow-none" data-bs-dismiss="modal" aria-label="Close"></button>';
    conteudoModal += '</div>';
    conteudoModal += '<div class="modal-body">';
    conteudoModal += '<object class="w-100 previa-pdf" data="' + $(this).data("id") + '"></object>';
    conteudoModal += '</div>';
    conteudoModal += '</div>';
    $("#modalConteudo").html(conteudoModal);
    $("#modalConteudo").addClass('modal-fullscreen');
    $(".modal").modal("show");
});

$(document).on("click", ".excluir-certificado", function(event) {
    event.preventDefault();
    var linha = $(this);
    $.ajax({
        url: "http://localhost:8888/candidato/certificado/excluir/" + Cookies.get("Id") + "/" + $(this).data("id"),
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

$(document).on("click", ".btn-close", function(event) {
    event.preventDefault();
    $("#modalConteudo").removeClass('modal-fullscreen');
});