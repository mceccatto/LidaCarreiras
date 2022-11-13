$("#formRegistrarCandidato input[name=telefone]").mask("(00)00000-0000", {
	placeholder: "(00)00000-0000"
});

$("#formRegistrarEmpresa input[name=cnpj]").mask('00.000.000/0000-00', {
    placeholder: "00.000.000/0000-00",
    reverse: true
});

$("#formRegistrarEmpresa input[name=telefone]").mask("(00)00000-0000", {
	placeholder: "(00)00000-0000"
});

$(document).on("click", ".criar-usuario", function(event) {
    event.preventDefault();
    $("#formEntrar").addClass("d-none");
    $("#formRecuperar").addClass("d-none");
    $("#formRegistrar").removeClass("d-none");
});

$(document).on("click", ".recuperar-acesso", function(event) {
    event.preventDefault();
    $("#formEntrar").addClass("d-none");
    $("#formRecuperar").removeClass("d-none");
    $("#formRegistrar").addClass("d-none");
});

$(document).on("click", ".voltar", function(event) {
    event.preventDefault();
    $(".alert").empty();
    $(".alert").addClass("d-none");
    $("#formRegistrar").addClass("d-none");
    $("#formRecuperar").addClass("d-none");
    $("#formEntrar").removeClass("d-none");
    $("select[name=perfil]").val("").change();
    $("form").trigger("reset");
});

$(document).on("change", "select[name=perfil]", function(event) {
    event.preventDefault();
    if ($("select[name=perfil] option:selected").val() == "") {
        $(".candidato").addClass("d-none");
        $(".empresa").addClass("d-none");
    }
    if ($("select[name=perfil] option:selected").val() == "C") {
        $(".candidato").removeClass("d-none");
        $(".empresa").addClass("d-none");
    }
    if ($("select[name=perfil] option:selected").val() == "E") {
        $(".candidato").addClass("d-none");
        $(".empresa").removeClass("d-none");
    }
});

$(document).on("submit", "#formEntrar", function(event) {
	event.preventDefault();
    $(".alert-entrar").empty();
    var urlApi,destino;
    if ($("#formEntrar select[name=perfilEntrar] option:selected").val() == "") {
        $(".alert-entrar").addClass("alert-danger");
        $(".alert-entrar").html("É obrigatório informar qual seu perfil!");
        $(".alert-entrar").removeClass("d-none");
        $("button").blur();
        return;
    } else {
        if ($("#formEntrar select[name=perfilEntrar] option:selected").val() == "C") {
            urlApi = "http://localhost:8888/candidato/login";
            destino = "restrito/feed/";
        } else {
            urlApi = "http://localhost:8888/empresa/login";
            destino = "restrito/empresa/";
        }
    }
    if ($("#formEntrar input[name=email]").val() == "" || $("#formEntrar input[name=senha]").val() == "") {
        $(".alert-entrar").addClass("alert-danger");
        $(".alert-entrar").html("Todos os campos são obrigatórios!");
        $(".alert-entrar").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: urlApi,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
			email:($("#formEntrar input[name=email]").val() != "") ? $("#formEntrar input[name=email]").val() : null,
			senha:($("#formEntrar input[name=senha]").val() != "") ? $("#formEntrar input[name=senha]").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
			$("button").blur();
			$("button").attr("disabled", true);
            $(".alert-entrar").removeClass("alert-success alert-danger alert-primary");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert-entrar").addClass("alert-danger");
					$(".alert-entrar").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert-entrar").addClass("alert-danger");
					$(".alert-entrar").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert-entrar").addClass("alert-success");
					$(".alert-entrar").html(retorno.MENSAGEM + " Aguarde... <span id='tempo'>3</span>");
					Cookies.set("Id", retorno.ID, { expires: 1 });
                    Cookies.set("Perfil", $("#formEntrar select[name=perfilEntrar] option:selected").val(), { expires: 1 });
					$("form").trigger("reset");
					tempo = 3;
                    redirecionamento = setInterval(function() {
                        tempo--;
                        $("#tempo").html(tempo);
                        if (tempo <= 0) {
                            clearInterval(redirecionamento);
                        }
                    }, 1000);
					setTimeout(function() {
						window.location.href = destino;
					}, 3000);
				break;
			}
            $(".alert-entrar").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
            $(".alert-entrar").addClass("alert-danger");
			$(".alert-entrar").html("Serviço temporariamente indisponível.");
            $(".alert-entrar").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
});

$(document).on("submit", "#formRegistrarCandidato", function(event) {
	event.preventDefault();
    $(".alert-registrar").empty();
    if($("#formRegistrarCandidato input[name=nome]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo nome completo é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato select[name=genero] option:selected").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("o campo gênero é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=dataNascimento]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo data de nascimento é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=telefone]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo telefone é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=email]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo e-mail é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=senha]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo senha é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=repitaSenha]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo repita senha é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarCandidato input[name=senha]").val() != $("#formRegistrarCandidato input[name=repitaSenha]").val()) {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("As senhas informadas não são idênticas!");
		$(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/candidato/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
			nome:($("#formRegistrarCandidato input[name=nome]").val() != "") ? $("#formRegistrarCandidato input[name=nome]").val() : null,
			nomeSocial:($("#formRegistrarCandidato input[name=nomeSocial]").val() != "") ? $("#formRegistrarCandidato input[name=nomeSocial]").val() : null,
			genero:($("#formRegistrarCandidato select[name=genero] option:selected").val() != "") ? $("#formRegistrarCandidato select[name=genero] option:selected").val() : null,
			dataNascimento:($("#formRegistrarCandidato input[name=dataNascimento]").val() != "") ? $("#formRegistrarCandidato input[name=dataNascimento]").val() : null,
			telefone:($("#formRegistrarCandidato input[name=telefone]").val() != "") ? $("#formRegistrarCandidato input[name=telefone]").val() : null,
			email:($("#formRegistrarCandidato input[name=email]").val() != "") ? $("#formRegistrarCandidato input[name=email]").val() : null,
			senha:($("#formRegistrarCandidato input[name=senha]").val() != "") ? $("#formRegistrarCandidato input[name=senha]").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert-registrar").removeClass("alert-success alert-danger alert-primary");
            $(".alert-registrar").addClass("alert-primary");
			$(".alert-registrar").html("Aguarde...");
			$(".alert-registrar").removeClass("d-none");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert-registrar").addClass("alert-danger");
					$(".alert-registrar").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert-registrar").addClass("alert-danger");
					$(".alert-registrar").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert-registrar").addClass("alert-success");
					$(".alert-registrar").html(retorno.MENSAGEM);
                    $("form").trigger("reset");
				break;
			}
            $(".alert-registrar").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert-registrar").addClass("alert-danger");
			$(".alert-registrar").html("Serviço temporariamente indisponível.");
            $(".alert-registrar").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
});

$(document).on("submit", "#formRegistrarEmpresa", function(event) {
	event.preventDefault();
    $(".alert-registrar").empty();
    if($("#formRegistrarEmpresa input[name=cnpj]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo cnpj é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=rasaoSocial]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo rasão social é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=nomeFantasia]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo nome fantasia é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=telefone]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo telefone é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=email]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo e-mail é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=senha]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo senha é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=repitaSenha]").val() == "") {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("O campo repita senha é obrigatório!");
        $(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
    if($("#formRegistrarEmpresa input[name=senha]").val() != $("#formRegistrarEmpresa input[name=repitaSenha]").val()) {
        $(".alert-registrar").addClass("alert-danger");
        $(".alert-registrar").html("As senhas informadas não são idênticas!");
		$(".alert-registrar").removeClass("d-none");
        $("button").blur();
        return;
    }
	$.ajax({
        url: "http://localhost:8888/empresa/registrar",
        type: "POST",
        dataType: "json",
        data: JSON.stringify({

            cnpj:($("#formRegistrarEmpresa input[name=cnpj]").val() != "") ? $("#formRegistrarEmpresa input[name=cnpj]").val() : null,
            rasaoSocial:($("#formRegistrarEmpresa input[name=rasaoSocial]").val() != "") ? $("#formRegistrarEmpresa input[name=rasaoSocial]").val() : null,
            nomeFantasia:($("#formRegistrarEmpresa input[name=nomeFantasia]").val() != "") ? $("#formRegistrarEmpresa input[name=nomeFantasia]").val() : null,
            telefone:($("#formRegistrarEmpresa input[name=telefone]").val() != "") ? $("#formRegistrarEmpresa input[name=telefone]").val() : null,
            ramal:($("#formRegistrarEmpresa input[name=ramal]").val() != "") ? $("#formRegistrarEmpresa input[name=ramal]").val() : null,
            email:($("#formRegistrarEmpresa input[name=email]").val() != "") ? $("#formRegistrarEmpresa input[name=email]").val() : null,
            senha:($("#formRegistrarEmpresa input[name=senha]").val() != "") ? $("#formRegistrarEmpresa input[name=senha]").val() : null
		}),
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("button").blur();
			$("button").attr("disabled", true);
            $(".alert-registrar").removeClass("alert-success alert-danger alert-primary");
            $(".alert-registrar").addClass("alert-primary");
			$(".alert-registrar").html("Aguarde...");
			$(".alert-registrar").removeClass("d-none");
		},
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert-registrar").addClass("alert-danger");
					$(".alert-registrar").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert-registrar").addClass("alert-danger");
					$(".alert-registrar").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert-registrar").addClass("alert-success");
					$(".alert-registrar").html(retorno.MENSAGEM);
                    $("form").trigger("reset");
				break;
			}
            $(".alert-registrar").removeClass("d-none");
			$("button").attr("disabled", false);
        },
		error: function() {
			$(".alert-registrar").addClass("alert-danger");
			$(".alert-registrar").html("Serviço temporariamente indisponível.");
            $(".alert-registrar").removeClass("d-none");
			$("button").attr("disabled", false);
        }
    });
});

$(document).on("submit", "#formRecuperar", function(event) {
    event.preventDefault();
    $(".alert-recuperar").empty();
    if($("#formRecuperar input[name=email]").val() == "") {
        $(".alert-recuperar").addClass("alert-danger");
        $(".alert-recuperar").html("É obrigatório informar um e-mail!");
		$(".alert-recuperar").removeClass("d-none");
        $("button").blur();
        return;
    }
    $(".alert-recuperar").removeClass("alert-success alert-danger alert-primary");
    $(".alert-recuperar").addClass("alert-primary");
	$(".alert-recuperar").html("Aguarde...");
	$(".alert-recuperar").removeClass("d-none");
    $.ajax({
        url: "http://localhost:8888/candidato/recuperar-senha/" + $("#formRecuperar input[name=email]").val(),
        type: "GET",
        dataType: "json",
        success: function (retorno) {
			switch(retorno.CODIGO) {
				case 1:
                    $(".alert-recuperar").addClass("alert-danger");
					$(".alert-recuperar").html(retorno.MENSAGEM);
				break;
				case 2:
                    $(".alert-recuperar").addClass("alert-danger");
					$(".alert-recuperar").html(retorno.MENSAGEM);
				break;
				default:
                    $(".alert-recuperar").addClass("alert-success");
                    $(".alert-recuperar").html(retorno.MENSAGEM);
                    $("form").trigger("reset");
				break;
			}
			$("button").attr("disabled", false);
        },
		error: function (retorno) {
            $(".alert-recuperar").addClass("alert-danger");
            if(retorno.status == 404) {
			    $(".alert-recuperar").html("É obrigatório informar um E-mail!");
            } else {
			    $(".alert-recuperar").html("Serviço temporariamente indisponível!");
            }
            $("button").attr("disabled", false);
        }
    });
});