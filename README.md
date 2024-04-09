# SAT Solver
## What is it
This project provides SAT solver.

## Getting Started
### Requirements
* python3
* picosat
* dotnet sdk

### Build
Firstly, clone this repository:
```
https://github.com/AlexanderYavorovsky/SATSolver
```
Then, navigate to project directory:
```
cd SATSolver
```
Finally, run this script to build the project and run tests:
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