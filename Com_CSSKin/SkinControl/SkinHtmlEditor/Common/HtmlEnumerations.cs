/********************************************************************
 * *
 * * 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
 * * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
 * *
 * * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
 * * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
 * * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
 * * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
 * *
 * * Copyright (C) 2013-? cskin Corporation All rights reserved.
 * * 网站：CSkin界面库 http://www.cskin.net
 * * 作者： 乔克斯 QQ：345015918 .Net项目技术组群：306485590
 * * 请保留以上版权信息，否则作者将保留追究法律责任。
 * *
 * * 创建时间：2016-01-18
 * * 说明：HtmlEnumerations.cs
 * *
********************************************************************/

namespace Com_CSSkin.SkinControl
{

    /// <summary>
    /// Enum used to insert a list
    /// </summary>
    public enum HtmlListType
    {
        Ordered,
        Unordered

    } //HtmlListType


    /// <summary>
    /// Enum used to insert a heading
    /// </summary>
    public enum HtmlHeadingType
    {
        H1 = 1,
        H2 = 2,
        H3 = 3,
        H4 = 4,
        H5 = 5

    } //HtmlHeadingType


    /// <summary>
    /// Enum used to define the navigate action on a user clicking a href
    /// </summary>
    public enum NavigateActionOption
    {
        Default,
        NewWindow,
        SameWindow

    } //NavigateActionOption


    /// <summary>
    /// Enum used to define the image align property
    /// </summary>
    public enum ImageAlignOption
    {
        AbsBottom,
        AbsMiddle,
        Baseline,
        Bottom,
        Left,
        Middle,
        Right,
        TextTop,
        Top

    } //ImageAlignOption


    /// <summary>
    /// Enum used to define the text alignment property
    /// </summary>
    public enum HorizontalAlignOption
    {
        Default,
        Left,
        Center,
        Right

    } //HorizontalAlignOption


    /// <summary>
    /// Enum used to define the vertical alignment property
    /// </summary>
    public enum VerticalAlignOption
    {
        Default,
        Top,
        Bottom

    } //VerticalAlignOption


    /// <summary>
    /// Enum used to define the visibility of the scroll bars
    /// </summary>
    public enum DisplayScrollBarOption
    {
        Yes,
        No,
        Auto

    } //DisplayScrollBarOption


    /// <summary>
    /// Enum used to define the unit of measure for a value
    /// </summary>
    public enum MeasurementOption
    {
        Pixel,
        Percent

    } //MeasurementOption

    /// <summary>
    /// Enumeration of possible font sizes for the Editor component
    /// </summary>
    public enum FontSize
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        NA
    }

    /// <summary>
    /// Enumeration of the Editor ready state
    /// </summary>
    public enum ReadyState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }

}