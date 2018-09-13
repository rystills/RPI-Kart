// Copyright 1998-2018 Epic Games, Inc. All Rights Reserved.

#include "RPIKartGameMode.h"
#include "RPIKartPawn.h"
#include "RPIKartHud.h"

ARPIKartGameMode::ARPIKartGameMode()
{
	DefaultPawnClass = ARPIKartPawn::StaticClass();
	HUDClass = ARPIKartHud::StaticClass();
}
