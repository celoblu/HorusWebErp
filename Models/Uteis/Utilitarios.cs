using HorusWebErp.Modelos;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace HorusWebErp.Uteis
{
    public class ValidaCNPJ
    {
        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

    }

    public class ValidaCPF
    {
        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }

    public class Suporte
    {
        public string RestaNumero(string Variavel)
        {
            if (Variavel == null)
            {
                return "";
            }
            string NovaVariavel = "";

            foreach (var item in Variavel)
            {
                int letra = Convert.ToInt16(item);

                if (letra >= 48 && letra <= 57)
                {
                    NovaVariavel += item;
                }
            }

            return NovaVariavel;
        }
    }

    public class CnpjWS
    {
        public CnpjModel ConsultarCnpj(string CnpjPesquisar)
        {
            CnpjModel empresa = BuscarCNPJ(CnpjPesquisar);
            return empresa;
        }

        public CnpjModel BuscarCNPJ(string Cnpj)
        {
            try
            {
                string EnderecoUrl = "https://www.receitaws.com.br/v1/cnpj/{0}";

                string NovoEnderecoURL = string.Format(EnderecoUrl, Cnpj);
                WebClient wc = new WebClient();
                string Conteudo = wc.DownloadString(NovoEnderecoURL);

                CnpjModel retorno = JsonConvert.DeserializeObject<CnpjModel>(Conteudo);

                return retorno;
            }
            catch (System.Exception e)
            {
                string erro = e.Message;
                CnpjModel retorno;
                retorno = new CnpjModel { };
                return retorno;
            }

        }

    }

    public class CepWS
    {
        private static string EnderecoUrl = "http://viacep.com.br/ws/{0}/json/";

        public static Endereco BuscarenderecoCEP(string cep)
        {
            string NovoEnderecoURL = string.Format(EnderecoUrl, cep);

            WebClient wc = new WebClient();
            string Conteudo = wc.DownloadString(NovoEnderecoURL); // metodo sincrono(tela fica travada) 

            Endereco end = JsonConvert.DeserializeObject<Endereco>(Conteudo);

            if (end.cep == null)
            {
                return null;
            }
            return end;
        }
    }

    // verifica se o modelo esta com todos os campos "null"
    public static class VerificarConteudo
    {
        public static bool VerificarPropriedadesNaoNulas<T>(this T obj)
        {
            return typeof(T).GetProperties().All(a => a.GetValue(obj) == null);
        }
    }


    public class Limpa
    {
        public string RestaNumero(string Variavel)
        {
            if (Variavel == null)
            {
                return "";
            }
            string NovaVariavel = "";

            foreach (var item in Variavel)
            {
                int letra = Convert.ToInt16(item);

                if (letra >= 48 && letra <= 57)
                {
                    NovaVariavel += item;
                }
            }

            return NovaVariavel;
        }
    }

}
