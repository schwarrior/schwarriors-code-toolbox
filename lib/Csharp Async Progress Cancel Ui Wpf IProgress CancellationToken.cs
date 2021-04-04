using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProgressAndCancelCSharpLab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CancellationTokenSource cancelTokenSource;

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnCancel.IsEnabled = true;
            tbTotalPrimeCount.IsEnabled = false;
            cancelTokenSource = new CancellationTokenSource();

            var primeCount = int.Parse(tbTotalPrimeCount.Text);
            var primesAbove = ulong.Parse(tbPrimesAbove.Text);
            var progress = new Progress<PrimeGeneratorProgress>();
            progress.ProgressChanged += Progress_ProgressChanged;
            var primes = await GetPrimesAsync(primeCount, primesAbove, progress, cancelTokenSource.Token);
            MessageBox.Show($"Finished. Max Prime: {primes.Max()}");

            btnStart.IsEnabled = true;
            btnCancel.IsEnabled = false;
            tbTotalPrimeCount.IsEnabled = true;
        }

        private void Progress_ProgressChanged(object sender, PrimeGeneratorProgress e)
        {
            tbCurrentIndex.Text = e.CurrentIndex.ToString();
            tbCurrentPrime.Text = e.CurrentPrime.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
            btnStart.IsEnabled = true;
            btnCancel.IsEnabled = false;
            tbTotalPrimeCount.IsEnabled = true;
        }

        private async Task<IEnumerable<ulong>> GetPrimesAsync(int primeCount, ulong primesAbove,
            IProgress<PrimeGeneratorProgress> progress, CancellationToken cancelToken)
        {
            if (primeCount < 1) throw new IndexOutOfRangeException();
            var foundPrimes = new List<ulong>();
            ulong? currentPrime = null;
            for (int index = 1; index < primeCount; index++)
            {
                currentPrime = await Task.Run(() => GetNextPrime(currentPrime ?? primesAbove, cancelToken));
                if (currentPrime == 0) return foundPrimes;
                foundPrimes.Add(currentPrime.Value);
                var progressDetail = new PrimeGeneratorProgress
                { CurrentPrime = currentPrime.Value, PrimeCount = foundPrimes.Count, CurrentIndex = index };
                progress.Report(progressDetail);
                if (cancelToken.IsCancellationRequested) break;
            }
            return foundPrimes;
        }

        private ulong GetNextPrime(ulong startingNumber, CancellationToken cancelToken)
        {
            var currentNumber = startingNumber;
            while (true)
            {
                currentNumber++;
                if (currentNumber == 1) return currentNumber;
                if (currentNumber % 2 == 0) continue;
                var isPrime = true;
                for (ulong divisor = 3; divisor < currentNumber; divisor += 2)
                {
                    if (currentNumber % divisor == 0) { isPrime = false; break; }
                    if (cancelToken.IsCancellationRequested) return 0;
                }
                if (isPrime) return currentNumber;
            }
        }
    }

    public class PrimeGeneratorProgress
    {
        public int PrimeCount { get; set; }
        public int CurrentIndex { get; set; }
        public ulong CurrentPrime { get; set; }
    }

}

//<Window x:Class="ProgressAndCancelCSharpLab01.MainWindow"
//        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
//        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
//        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
//        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
//        xmlns:local="clr-namespace:ProgressAndCancelCSharpLab01"
//        mc:Ignorable="d"
//        Title="MainWindow" Height="300" Width="300" MinWidth="250"
//    >

//    <Grid Margin = "3,3,10,3" >
//        < Grid.RowDefinitions >
//            < RowDefinition Height="Auto"></RowDefinition>
//            <RowDefinition Height = "Auto" ></ RowDefinition >
//            < RowDefinition Height="Auto"></RowDefinition>
//            <RowDefinition Height = "Auto" ></ RowDefinition >
//            < RowDefinition Height="Auto"></RowDefinition>
//            <RowDefinition Height = "Auto" ></ RowDefinition >
//        </ Grid.RowDefinitions >
//        < Grid.ColumnDefinitions >
//            < ColumnDefinition Width="Auto"></ColumnDefinition>
//            <ColumnDefinition Width = "*" MinWidth="50" MaxWidth="800"></ColumnDefinition>
//        </Grid.ColumnDefinitions>

//        <Label Grid.Row="0" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Primes Above:</Label>
//        <TextBox x:Name= "tbPrimesAbove" Grid.Row= "0" Grid.Column= "1" Margin= "3"
//             Height= "Auto"  VerticalAlignment= "Center" > 100000000 </ TextBox >


//        < Label Grid.Row= "1" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Total Prime Count:</Label>
//        <TextBox x:Name= "tbTotalPrimeCount" Grid.Row= "1" Grid.Column= "1" Margin= "3"
//             Height= "Auto"  VerticalAlignment= "Center" > 100 </ TextBox >


//        < Label Grid.Row= "2" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Start:</Label>
//        <Button x:Name= "btnStart" Grid.Row= "2" Grid.Column= "1" Margin= "3" Padding= "2" Click= "btnStart_Click" > Start </ Button >


//        < Label Grid.Row= "3" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Current Index:</Label>
//        <TextBox x:Name= "tbCurrentIndex" IsReadOnly= "True" Grid.Row= "3" Grid.Column= "1" Margin= "3"
//             Height= "Auto"  VerticalAlignment= "Center" ></ TextBox >


//        < Label Grid.Row= "4" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Current Prime:</Label>
//        <TextBox x:Name= "tbCurrentPrime" IsReadOnly= "True" Grid.Row= "4" Grid.Column= "1" Margin= "3"
//             Height= "Auto"  VerticalAlignment= "Center" ></ TextBox >

//        < Label Grid.Row= "5" Grid.Column= "0" Margin= "3"
//             VerticalAlignment= "Center" > Start:</Label>
//        <Button x:Name= "btnCancel" IsEnabled= "False" Grid.Row= "5" Grid.Column= "1" Margin= "3" Padding= "2" Click= "btnCancel_Click" > Cancel </ Button >

//    </ Grid >


//</ Window >