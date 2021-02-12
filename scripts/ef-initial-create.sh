#!/bin/bash

dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate --project budget4home --context Context