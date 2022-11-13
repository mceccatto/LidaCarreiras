using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace LidaCarreiras.Classes
{
    public class ExecucoesDiversas
    {
        public static JsonObject ConsultaSelectVaga()
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            MySqlConnection conexao = new MySqlConnection(conexaoString);
            conexao.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT * FROM vagas_modalidade ORDER BY Modalidade ASC";
            cmd.Prepare();
            var executa = cmd.ExecuteReader();
            JsonArray modalidadesArray = new JsonArray();
            if (executa.HasRows)
            {
                while (executa.Read())
                {
                    JsonObject modalidadeTemp = new JsonObject();
                    modalidadeTemp.Add("Id", long.Parse(executa.GetString("Id")));
                    modalidadeTemp.Add("Modalidade", executa.GetString("Modalidade"));
                    modalidadesArray.Add(modalidadeTemp);
                }
                retornoJson.Add("MODALIDADES", modalidadesArray);
            }
            cmd.Connection.Close();
            conexao = new MySqlConnection(conexaoString);
            conexao.Open();
            cmd = new MySqlCommand();
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT * FROM vagas_modelo ORDER BY Modelo ASC";
            cmd.Prepare();
            executa = cmd.ExecuteReader();
            JsonArray modelosArray = new JsonArray();
            if (executa.HasRows)
            {
                while (executa.Read())
                {
                    JsonObject modeloTemp = new JsonObject();
                    modeloTemp.Add("Id", long.Parse(executa.GetString("Id")));
                    modeloTemp.Add("Modelo", executa.GetString("Modelo"));
                    modelosArray.Add(modeloTemp);
                }
                retornoJson.Add("MODELOS", modelosArray);
            }
            cmd.Connection.Close();
            conexao = new MySqlConnection(conexaoString);
            conexao.Open();
            cmd = new MySqlCommand();
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT * FROM vagas_area ORDER BY Area ASC";
            cmd.Prepare();
            executa = cmd.ExecuteReader();
            JsonArray areasArray = new JsonArray();
            if (executa.HasRows)
            {
                while (executa.Read())
                {
                    JsonObject areaTemp = new JsonObject();
                    areaTemp.Add("Id", long.Parse(executa.GetString("Id")));
                    areaTemp.Add("Area", executa.GetString("Area"));
                    areasArray.Add(areaTemp);
                }
                retornoJson.Add("AREAS", areasArray);
            }
            cmd.Connection.Close();
            return retornoJson;
        }

        public static JsonObject ConsultaVagasIds()
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT Id FROM vagas ORDER BY DataCadastro DESC";
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonArray vagaArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        vagaArray.Add(long.Parse(executa.GetString("Id")));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", vagaArray);
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Nenhum registro foi encontrado!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject ConsultaVagaId(long id)
        {
            var culture = new CultureInfo("pt-BR", false);
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT A.Empresa AS EmpresaId, B.NomeFantasia AS EmpresaNome, B.Descricao AS EmpresaDescricao, A.Titulo, A.Descricao, A.Modalidade AS ModalidadeId, C.Modalidade, A.Modelo AS ModeloId, D.Modelo, A.Area AS AreaId, E.Area, A.DataLimite, A.Status, A.DataCadastro FROM vagas A LEFT JOIN empresas B ON B.Id = A.Empresa LEFT JOIN vagas_modalidade C ON C.Id = A.Modalidade LEFT JOIN vagas_modelo D ON D.Id = A.Modelo LEFT JOIN vagas_area E ON E.Id = A.Area WHERE A.Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject vaga = new JsonObject();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        vaga.Add("Id", id);
                        vaga.Add("EmpresaId", long.Parse(executa.GetString("EmpresaId")));
                        vaga.Add("EmpresaNome", executa.GetString("EmpresaNome"));
                        vaga.Add("EmpresaDescricao", executa.GetString("EmpresaDescricao"));
                        vaga.Add("Titulo", executa.GetString("Titulo"));
                        vaga.Add("Descricao", executa.GetString("Descricao"));
                        vaga.Add("ModalidadeId", long.Parse(executa.GetString("ModalidadeId")));
                        vaga.Add("Modalidade", executa.GetString("Modalidade"));
                        vaga.Add("ModeloId", long.Parse(executa.GetString("ModeloId")));
                        vaga.Add("Modelo", executa.GetString("Modelo"));
                        vaga.Add("AreaId", long.Parse(executa.GetString("AreaId")));
                        vaga.Add("Area", executa.GetString("Area"));
                        vaga.Add("DataLimite", (!executa.IsDBNull(executa.GetOrdinal("DataLimite"))) ? DateTime.ParseExact(executa.GetString("DataLimite"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd") : null);
                        vaga.Add("Status", Convert.ToBoolean(executa.GetString("Status")));
                        vaga.Add("DataCadastro", executa.GetString("DataCadastro"));
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", vaga);
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Nenhum registro foi encontrado!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject CurtirVaga(long candidato, long vaga)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM curtidas WHERE Vaga = @Vaga AND Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Vaga", vaga);
                cmd.Parameters.AddWithValue("@Candidato", candidato);
                cmd.Prepare();
                var executaBusca = cmd.ExecuteReader();
                if (executaBusca.HasRows)
                {
                    cmd.Connection.Close();
                    conexao = new MySqlConnection(conexaoString);
                    conexao.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = conexao;
                    cmd.CommandText = "DELETE FROM curtidas WHERE Vaga = @Vaga AND Candidato = @Candidato";
                    cmd.Parameters.AddWithValue("@Vaga", vaga);
                    cmd.Parameters.AddWithValue("@Candidato", candidato);
                    cmd.Prepare();
                    var executa = cmd.ExecuteNonQuery();
                    if (executa > 0)
                    {
                        retornoJson.Add("CODIGO", 0);
                        retornoJson.Add("STATUS", "descurtida");
                        retornoJson.Add("MENSAGEM", "Vaga descurtida com sucesso!");
                    }
                    else
                    {
                        retornoJson.Add("CODIGO", 1);
                        retornoJson.Add("MENSAGEM", "Não foi possível descurtir no momento!");
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    cmd.Connection.Close();
                    conexao = new MySqlConnection(conexaoString);
                    conexao.Open();
                    cmd = new MySqlCommand();
                    cmd.Connection = conexao;
                    cmd.CommandText = "INSERT INTO curtidas (Vaga,Candidato,DataCadastro) VALUES (@Vaga,@Candidato,@DataCadastro)";
                    cmd.Parameters.AddWithValue("@Vaga", vaga);
                    cmd.Parameters.AddWithValue("@Candidato", candidato);
                    cmd.Parameters.AddWithValue("@DataCadastro", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                    cmd.Prepare();
                    var executa = cmd.ExecuteNonQuery();
                    if (executa > 0)
                    {
                        retornoJson.Add("CODIGO", 0);
                        retornoJson.Add("STATUS", "curtida");
                        retornoJson.Add("MENSAGEM", "Vaga curtida com sucesso!");
                    }
                    else
                    {
                        retornoJson.Add("CODIGO", 1);
                        retornoJson.Add("MENSAGEM", "Não foi possível curtir no momento!");
                    }
                    cmd.Connection.Close();
                }
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }

        public static JsonObject ConsultaCurtidas(long candidato, long vaga)
        {
            var culture = new CultureInfo("pt-BR", false);
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM curtidas WHERE Vaga = @Vaga AND Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Vaga", vaga);
                cmd.Parameters.AddWithValue("@Candidato", candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                if (executa.HasRows)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Vaga já curtida");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Vaga não curtida!");
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }
    }
}
