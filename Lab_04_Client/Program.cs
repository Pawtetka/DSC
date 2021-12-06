// See https://aka.ms/new-console-template for more information

double f(double x) => Math.Pow(x, 3) * Math.Sqrt(Math.Pow(Math.Pow(x, 2) + 4, 3));
double Function(double x) => (Math.Sqrt(Math.Pow(Math.Pow(x, 2) + 4, 5)) * (Math.Pow(x, 2) - (8 / 5))) / 7;
const int xMin = 3;
const int xMax = 5;
const double deltaX = 0.0001;

MPI.Environment.Run(ref args, comm =>
{
    var values = new Tuple<int, int>(xMin, xMax);
    comm.Broadcast<Tuple<int, int>>(ref values, 0);
    if(comm.Rank is 0)
    {
        var n = (values.Item2 - values.Item1) / deltaX;
        var a = values.Item1 + deltaX * comm.Rank * n / comm.Size;
        var b = a + deltaX * n / comm.Size;
        var sum = RightRectangle(f, a, b, (int)n / comm.Size);
        
        var total = comm.Reduce(sum, MPI.Operation<double>.Add, 0);
        Console.WriteLine($"Sum elements of array is {total}");
        Console.WriteLine($"F(x) where x = 5 is {Function(5)}");
    }
    else
    {
        var n = (values.Item2 - values.Item1) / deltaX;
        var a = values.Item1 + deltaX * comm.Rank * n / comm.Size;
        var b = a + deltaX * n / comm.Size;
        var sum = RightRectangle(f, a, b, (int)n / comm.Size);
        
        comm.Reduce(sum, MPI.Operation<double>.Add, 0);
    }
});

static double RightRectangle(Func<double, double> f, double a, double b, int n)
{
    var h = (b - a) / n;
    var sum = 0d;
    for (var i = 1; i <= n; i++)
    {
        var x = a + i * h;
        sum += f(x);
    }

    var result = h * sum;
    return result;
}