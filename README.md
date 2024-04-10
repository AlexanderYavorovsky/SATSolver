# SAT Solver
## What is it
This project provides SAT solver.

## Getting Started
### Requirements
* Python 3.10+
* picosat
* dotnet SDK 7.0+

### Build
Firstly, clone this repository:
```
https://github.com/AlexanderYavorovsky/SATSolver
```
Then, navigate to project directory:
```
cd SATSolver
```
Build project:
```
dotnet build -c Release
```
Finally, run this script to run tests:
``` 
python3 testall.py
```

## Usage
Input: .txt file in DIMACS format.
Output: SAT/UNSAT + model (if SAT).

Consider the following usage example:
```
dotnet run dimacs1.txt
```