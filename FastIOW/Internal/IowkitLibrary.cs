﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tederean.FastIOW.Internal
{

  /// <summary>
  /// Wrapper class for native iowkit dynamic link library.
  /// Use it only by yourself if you know what you are doing.
  /// </summary>
  public static class IowkitLibrary
  {

    public const string NativeLibraryName = "iowkit";


    /// <summary>
    /// Die Metode IowKitOpenDevice öffnet alle am USB erkannten IO-Warrior-Bausteine
    /// und liefert ein Handle für den zuerst gefundenen IO-Warrior zurück.
    /// </summary>
    /// <returns>Handle für IO-Warrior oder null bei Misserfolg.</returns>
    [DllImport(NativeLibraryName)]
    public static extern IntPtr IowKitOpenDevice();


    /// <summary>
    /// Die Methode IowKitCloseDevice schließt den IO-Warrior der durch das
    /// gegebene Handle kontrolliert wird.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    [DllImport(NativeLibraryName)]
    public static extern void IowKitCloseDevice(IntPtr IowHandle);


    /// <summary>
    /// Fordert die Produkt-Identität (PID) des IO-Warrior Bausteines ab,
    /// der durch das gegebene Handle kontrolliert wird.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitGetProductId(IntPtr IowHandle);


    /// <summary>
    /// Zählt und nummeriert die am USB angeschlossenen IO-Warrior Bausteine.
    /// </summary>
    /// <returns></returns>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitGetNumDevs();


    /// <summary>
    /// Die Methode IowKitGetDeviceHandle arbeitet liefert zu dem Gerät mit
    /// der angegebenen Nummer ein gültiges Handle. Vorrausgesetzt, das Gerät
    /// mit der Nummer existiert.
    /// IowKitGetDeviceHandle funktioniert nur nach einem erfolgreichen Aufruf
    /// von IowKitDeviceOpen. Erst dann sind alle Bausteine nummeriert.
    /// </summary>
    /// <param name="numDevice">Nummer des zu öffnenden Gerätes.</param>
    /// <returns>Ein gültiges Handle für das Gerät oder 0 bei Misserfolg.</returns>
    [DllImport(NativeLibraryName)]
    public static extern IntPtr IowKitGetDeviceHandle(UInt32 numDevice);


    /// <summary>
    /// Liefert die Version der IoWarrior-Firmaware zurück. Die oberen 2 Byte sind immer 0.
    /// Es sind 4 Hex-Stellen gültig. Wäre die gegenwärtige Softwareversion 1.0.2.1 so wird
    /// 0x1021 zurückgegeben. Ältere IO-Warrior, die keine eigene Seriennummer haben, liefern
    /// eine Versionsnummer kleiner 1.0.1.0 zurück.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <returns>Die Softwareversion des IO-Warriors oder 0, wenn kein gültiges
    /// Handle übergeben wurde.</returns>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitGetRevision(IntPtr IowHandle);


    /// <summary>
    /// Liest die Seriennummer aus dem durch ein gültiges Handle gesteuerten IO-Warrior
    /// heraus.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="SnTarget">Verweis auf ein Datenfeld, dass die Seriennummer speichert.
    /// Der Stringbuilder muss zuvor instanziert worden sein.</param>
    /// <returns>True, wenn die Seriennummer erfolgreich im angegebenen Datenfeld abgelegt
    /// werden konnte. False, wenn das Handle ungültig war, der IO-Warrior keine Seriennummer
    /// hat oder das Abspeichern der Nummer im übergebenen Ziel fehlgeschlagen ist.</returns>
    [DllImport(NativeLibraryName, CharSet = CharSet.Unicode)]
    public static extern bool IowKitGetSerialNumber(IntPtr IowHandle, StringBuilder SnTarget);


    /// <summary>
    /// Liest Daten vom IO-Warrior. Die Funktion blockiert den aufrufenden Thread bis
    /// eine Änderung an den Eingängen des IO-Warriors durch den Baustein regisriert wird.
    /// Ein nicht blockierendes Lesen kann über den Aufruf von IowKitReadNonBlocking()
    /// erfolgen.
    /// Die Daten kommen in Form eines Reports an. Nähere Beschreibung der Reports ist
    /// im Datenblatt des jeweiligen IO-Warriors zu finden.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="numPipe">Datenkanal, aus dem gelesen werden soll. Reicht von 0
    /// bis IOWKIT_MAX_PIPES-1</param>
    /// <param name="buffer">Ziel für Ablage der Daten.</param>
    /// <param name="Length">Anzahl der einzulesenden Bytes.</param>
    /// <returns>Anzahl tatsächlich eingelesener Bytes oder 0 bei Misserfolg.</returns>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitRead(IntPtr IowHandle, UInt32 numPipe, byte[] buffer, UInt32 Length);


    /// <summary>
    /// Liest Daten vom IO-Warrior. Der aufrufende Thread wird nicht blockiert, wenn
    /// keine Daten zur Verfügung stehen, also keine Änderung an den Eingängen des
    /// IO-Warriors stattgefunden hat.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="numPipe">Datenkanal, aus dem gelesen werden soll. Reicht von 0
    /// bis IOWKIT_MAX_PIPES-1</param>
    /// <param name="buffer">Ziel für Ablage der Daten.</param>
    /// <param name="Length">Anzahl der einzulesenden Bytes.</param>
    /// <returns>Anzahl tatsächlich eingelesener Bytes oder 0 bei Misserfolg.</returns>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitReadNonBlocking(IntPtr IowHandle, UInt32 numPipe, byte[] buffer, UInt32 Length);


    /// <summary>
    /// Liest den Pinstatus der Eingänge des angeschlossenen IO-Warriors direkt aus.
    /// Die Funktion gibt True zurück, wenn zuvor eine Änderung an den Pins durch den
    /// Baustein registriert wurde. Gleichzeitig is im Ziel-Datenfeld ein Abbild der
    /// Eingänge als Wert verfügbar.
    /// Achtung!!: Diese Funktion arbeitet nicht mit dem IO-Warrior 56 zusammen. Der
    /// Baustein gibt mehr als 32-Bit zurück.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="IoPinStatus">Ziel-Datenfeld für das Abspeichern der Pin-Zustände.</param>
    /// <returns>True bei Erfolg, False wenn keine neuen Daten erfasst wurden oder
    /// das übergebene Handle nicht funktioniert.</returns>
    [DllImport(NativeLibraryName)]
    public static extern bool IowKitReadImmediate(IntPtr IowHandle, byte[] IoPinStatus);


    /// <summary>
    /// Legt die Grenze für eine Zeitüberschreitung beim Lesezugriff auf den IO-Warrior fest.
    /// Ein Timeout während einer laufenden, noch nicht beendeten Leseaktion führt zu
    /// Datenverlust (Reports gehen verloren). IowKitRead() schlägt fehl, wenn nicht innerhalb
    /// der gegebenen Grenze Daten vom IO-Warrior eingelesen werden können.
    /// Bei Zeitüberschreitung müssen alle vor bis zu dem Zeitpunkt bereits gesendeten
    /// aber noch nicht ausgeführten Aktionen erneut übermittelt werden (z.B. I2C-Bus schreiben
    /// oder lesen).
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="Timeout">Grenze für Zeitüberschreitung in Millisekunden. Empfohlen in
    /// der iowkit.dll-Beschreibung sind 1000ms oder mehr.</param>
    /// <returns>True bei Erfolg oder False bei Fehlschlag.</returns>
    [DllImport(NativeLibraryName)]
    public static extern bool IowKitSetTimeout(IntPtr IowHandle, UInt32 Timeout);


    /// <summary>
    /// Legt die Grenze für eine Zeitüberschreitung beim Schreibzugriff auf den IO-Warrior fest.
    /// IowKitWrite() schlägt fehl, wenn Daten nicht innerhalb der vorgegebenen Grenze geschrieben
    /// werden können. Schreibfehler sind untypisch. Verbindungen und Hardware näher prüfen,
    /// wenn Fehler auftreten.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="Timeout">Grenze für Zeitüberschreitung in Millisekunden. Empfohlen in
    /// der iowkit.dll-Beschreibung sind 1000ms oder mehr.</param>
    /// <returns>True bei Erfolg oder False bei Fehlschlag.</returns>
    [DllImport(NativeLibraryName)]
    public static extern bool IowKitSetWriteTimeout(IntPtr IowHandle, UInt32 Timeout);


    /// <summary>
    /// Beendet alle laufenden Datentransferaktionen auf dem angegebenen Datenkanal(Pipe).
    /// Der Einsatz dieser Methode erfordert im Regelfall einen eigenen Thread, da eine
    /// beipsielsweise laufende IowKitRead()-Funktion den aufrufenden Thread blockiert,
    /// bis eine Rückgabe empfangen werden konnte.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="numPipe">Nummer des Datenkanals.</param>
    /// <returns>True bei Erfolg, sonst false.</returns>
    [DllImport(NativeLibraryName)]
    public static extern bool IowKitCancelIo(IntPtr IowHandle, UInt32 numPipe);


    /// <summary>
    /// Schreibt "length" Bytes in den durch "numPipe" angegebenen Datenkanal des IO-Warriors.
    /// Wird etwas anderes als ein gültiger Report, bestehend aus einer korrekten Anzahl von
    /// Bytes und einer vom gewählten Datenkanal akzeptierten Report-ID, gesendet, schlägt
    /// die Funktion fehl.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <param name="numPipe">Datenkanal, der beschickt werden soll.</param>
    /// <param name="buffer">Puffer, der die zu schreibenden Daten bereistellt.</param>
    /// <param name="length">Anzahl zu schreibender Bytes.</param>
    /// <returns>Bei erfolgreicher Durchführung ist die Anzahl geschriebener =
    /// Anazahl zu schreibender Bytes.</returns>
    [DllImport(NativeLibraryName)]
    public static extern uint IowKitWrite(IntPtr IowHandle, UInt32 numPipe, byte[] buffer, UInt32 length);


    /// <summary>
    /// Gibt den Handle des Threads zurück, der zum Lesen der I/O-Pins des
    /// IO-Warriors genutzt wird. Linux nutzt keie Threads um die Funktionen
    /// des IO-Warriors einzubinden und gibt daher immer 0 zurück.
    /// </summary>
    /// <param name="IowHandle">Gültiges Handle zum IO-Warrior.</param>
    /// <returns>Handle für den laufenden Thread oder 0 bei Misserfolg.</returns>
    [DllImport(NativeLibraryName)]
    public static extern IntPtr IowKitGetThreadHandle(IntPtr IowHandle);


    /// <summary>
    /// Gibt die Version der iowkit.dll als string zurück.
    /// </summary>
    /// <returns>String mit der Versionsangabe.</returns>
    [DllImport(NativeLibraryName)]
    public static extern string IowKitVersion();


    /// <summary>
    /// Öffnet einen IOWarrior40 dessen Firmware Revision älter 1.0.1.0 ist. Diese
    /// Bausteine haben keine Seriennummer die ausgelesen werden kann.
    /// </summary>
    /// <param name="legacyOpenMode">Simple oder Complex-Mode.</param>
    /// <returns>True bei Erfolg sonst false.</returns>
    [DllImport(NativeLibraryName)]
    public static extern bool IowKitSetLegacyOpenMode(UInt32 legacyOpenMode);
  }
}
