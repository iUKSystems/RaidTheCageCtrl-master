using System;

public enum eCountries
{
    UK = 0,  
    TURKEY = 1,
    RUSSIA = 2,
    ROMANIA = 3,
    ARGENTINA = 4,
    URUGUAY = 5,
    PERU = 6,
    MEXICO = 7,
    COLUMBIA = 8,
    VIETNAM = 9,
    PARAQUAY =10,
    PORTUGAL = 11,
    HUNGARY = 12,
    BRAZIL = 13,
    URUGUAY2018 = 14,
};

public enum eStackTypes : int
{
    Main = 0,
    Switch = 1,    
}


public enum eSortItems
{
    RANDOM,
    HIGHTOLOW,
    LOWTOHIGH,
}

public enum eEngines : int
{
    OVERLAY = 0,
    PROJECTOR = 1,            
    WISEQ = 2,
    WISEAUDIO = 3,
    HOST = 4,
    CAGE = 5,
    HOSTMESSAGING = 6,      // will be a socketdeamon!  // so port to listen on only
    MIDI = 7,
    EXTRAQA = 8,
    MAX = 9,
};

public enum eSpecialEngines : int
{    
    HOSTMESSAGING, // socket 
}



