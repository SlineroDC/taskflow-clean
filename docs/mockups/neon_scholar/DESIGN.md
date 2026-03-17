# Design System Specification: High-Performance Editorial

## 1. Overview & Creative North Star: "The Kinetic Academy"
This design system is built to bridge the gap between high-stakes project management and the rapid-growth energy of tech education. Our Creative North Star is **"The Kinetic Academy."** 

Unlike static, grid-bound SaaS platforms, this system prioritizes a sense of "intellectual momentum." We move beyond the "template" look by utilizing intentional asymmetry, overlapping container logic, and a high-contrast typographic scale. The interface should feel like a premium digital terminal—authoritative, developer-centric, and deeply immersive. We reject "flat" design in favor of "tonal depth," where the UI feels like layered sheets of obsidian and smoked glass, punctuated by radioactive neon accents.

---

## 2. Colors: Tonal Depth & Neon Precision
Our palette is rooted in deep slates and navies, designed to reduce eye strain during long "deep work" sessions, with a signature "Neon Green" used exclusively for high-intent actions.

### The "No-Line" Rule
**Explicit Instruction:** Designers are prohibited from using 1px solid borders for general sectioning or layout division. Boundaries must be defined solely through background color shifts.
*   *Example:* A `surface-container-low` section sitting directly on a `surface` background provides enough contrast to define a zone without the visual "noise" of a stroke.

### Surface Hierarchy & Nesting
Treat the UI as a series of physical layers. We use the Material surface tiers to create "nested" depth:
- **Background (`surface` / #060e1f):** The foundation.
- **Sidebars/Navigation (`surface-container-low` / #0a1327):** Subordinate to the main stage.
- **Primary Work Cards (`surface-container` / #0f192f):** Where the user focus lies.
- **Popovers/Modals (`surface-container-highest` / #1a253f):** The "top-most" layer, often utilizing glassmorphism.

### The "Glass & Gradient" Rule
To achieve a signature premium feel, floating elements (Modals, Tooltips, Hover States) should utilize **Glassmorphism**:
- **Background:** `surface-variant` at 60% opacity.
- **Backdrop Blur:** 12px – 20px.
- **Signature Texture:** Use a subtle linear gradient on Primary CTAs transitioning from `primary` (#bcf161) to `primary-container` (#7faf25) at a 135-degree angle. This adds "soul" and prevents the neon from looking like a flat web default.

---

## 3. Typography: Editorial Authority
We utilize a dual-font strategy to balance technical precision with high-end editorial impact.

- **Display & Headlines (Manrope):** Chosen for its geometric modernism and slightly wider stance. Use `display-lg` and `headline-md` to create "Hero" moments in the dashboard.
- **UI & Body (Inter):** The workhorse. Inter’s high x-height ensures legibility in dense project data.
- **Visual Hierarchy:** Maintain a massive contrast between `display-lg` (3.5rem) for page titles and `label-sm` (0.6875rem) for metadata. This "Big & Small" approach creates the editorial look.

---

## 4. Elevation & Depth: Tonal Layering
Traditional drop shadows are largely replaced by **Tonal Layering**.

- **The Layering Principle:** Depth is achieved by stacking. Place a `surface-container-lowest` (#000000) element inside a `surface-container` (#0f192f) to create a "recessed" well for code snippets or data logs.
- **Ambient Shadows:** When an element *must* float (e.g., a dragged task card), use an extra-diffused shadow: `box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4)`. The shadow should feel like ambient occlusion, not a harsh drop shadow.
- **The "Ghost Border" Fallback:** If accessibility requires a border, use the "Ghost" method: `outline-variant` (#40485c) at **15% opacity**. This creates a suggestion of a container without breaking the "No-Line" rule.

---

## 5. Components

### Buttons
- **Primary:** `primary` background, `on_primary_container` text. 
  - **State:** On hover, apply `shadow-[0_0_20px_rgba(152,202,63,0.4)]` and `scale-105`.
- **Secondary:** Transparent background with a `Ghost Border`. Text in `primary`.
- **Tertiary:** Text-only in `on_surface_variant`, shifting to `on_surface` on hover.

### Cards & Task Items
- **Rule:** No divider lines. Use `spacing-6` (1.5rem) to separate content blocks. 
- **Interaction:** On hover, cards transition their ghost border to 40% opacity and apply a 2px left-border accent in `primary`.

### Input Fields
- **Styling:** `surface-container-high` background, `none` border.
- **Focus:** 2px ring in `primary` with a 4px soft outer glow.
- **Label:** Always use `label-md` in `on_surface_variant` for a "dev-tool" aesthetic.

### Additional Signature Components
- **The Progress Rail:** Instead of a standard bar, use a 2px high rail using `outline-variant` with a `primary` glow for the active state.
- **Status Micro-Chips:** Small, uppercase `label-sm` text with a 4px solid circle indicator. Avoid heavy background fills on chips to keep the UI "light."

---

## 6. Do’s and Don’ts

### Do:
- **Use "Intentional Asymmetry":** Offset your column widths (e.g., a 7-column main area and a 3-column "Education/Resources" sidebar).
- **Embrace "Dark Space":** Use `spacing-12` or `spacing-16` between major sections to let the high-end typography breathe.
- **Layer with Purpose:** Only use the `highest` surface container for elements that require immediate user intervention (Modals/Critical Alerts).

### Don't:
- **Don't use 100% white (#FFFFFF):** Use `on_surface` (#dee5ff) for text. Pure white is too harsh against the deep navy background.
- **Don't use high-contrast borders:** It breaks the "Kinetic Academy" immersion and makes the app look like a legacy bootstrap site.
- **Don't over-saturate:** Keep the neon `primary` color for "Action" and "Success." For data visualization, use the `secondary` and `tertiary` muted tones to avoid a "Christmas tree" effect.