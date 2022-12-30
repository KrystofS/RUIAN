# RUIAN

Simple RUIAN API .NET library. This project is by no means a complete implementation. It originated as a part of a larger project I decided to open source. This is in no way an official library. Despite the fact that the code itself is quite simple, this repository can be a good starting point for any other implementation, as there is little information about the RUIAN API online. Additional information can be found in the [wiki](https://github.com/KrystofS/RUIAN/wiki).

The original ArcGIS RUIAN REST API is available [here](https://ags.cuzk.cz/arcgis/rest/services/RUIAN).

Documentation is available [here](https://krystofs.github.io/RUIAN/index.html).

## Disclaimers and notes
- SimpleRUIANQueryBuider was created to fit the needs of a specific project while initially having higher ambitions. As such, it is admittedly not broadly usable, and any potential user should be aware of its severe limitations. The RUIAN REST API allows for much more flexibility than SimpleRUIANQueryBuider offers.
- As the domain is inherently based in Czech, I decided not to translate much of the properties. Even though it might seem a little odd to have English object names and Czech properties, I think this is the lesser evil. Where I did decide to translate the terminology in part for legacy reasons of consistency with the original project, I decided not to waste time searching for proper terms and coined some of my own translations. Although I believe that these are very intuitive, the reader should be at least aware that these are not in any way official translations.