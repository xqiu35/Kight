FAST HIGHIGHT MANUAL
====================

Usage Instructions
------------------
To use the Fast Highlight plug-in, simply add the Highlighter component over the object you wish to be highlighted. To turn on the highlight outline just call the Highlighter#ConstantOn() method, passing a Color as an argument and optionally a float represeting the thickness of the highlight outline.

Turning off the highlight outline can be achieved by calling the Highlighter#ConstantOff() method.

While the highlight is on you can fine tune it from the Editor or from its various exposed properties:

- HighlightColor: controlls the color of the outline
- HighlightThickness: controlls the thickness of the highlight outline
- IsFluidGeometry: set this to true if you have a model which has a complicated fluid geometry. When this property is set, the Highlighter component uses the models normals to calculate the highlight which results in a small increse in the computational footprint but a much better highlight outline.