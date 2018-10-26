namespace WSDLTester.Classes
{
    class Alumno
    {
        private string matricula;
        private string primerApellido;
        private string segundoApellido;
        private string nombre;
        private string nomDependencia;
        private string cveDependencia;
        private string cveUnidad;

        public Alumno(string m, string ap, string am, string n, string cd, string cu, string d)
        {
            this.matricula = m;
            this.primerApellido = ap;
            this.segundoApellido = am;
            this.nombre = n;
            this.nomDependencia = d;
            this.cveDependencia = cd;
            this.cveUnidad = cu;
        }

        public string GetNombreCompleto()
        {
            string name = this.nombre + " " + this.primerApellido + " " + this.segundoApellido;
            return name;
        }

        public string GetNombre()
        {
            return this.nombre;
        }

        public string GetPrimerApellido()
        {
            return this.primerApellido;
        }

        public string GetSegundoApellido()
        {
            return this.segundoApellido;
        }

        public bool CompararDatos(string m, string n, string pa, string sa)
        {
            if (m == this.matricula && n == this.nombre && pa == this.primerApellido && sa == this.segundoApellido)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDependencia()
        {
            return this.nomDependencia;
        }
    }
}