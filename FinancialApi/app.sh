#!/usr/bin/env bash

dotnet build -c Release -o ./bin/ &&
dotnet ./bin/FinancialApi.dll
